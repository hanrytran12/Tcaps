using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Command.AddProductionReport
{
    public class AddProductionReportCommandHandler : IRequestHandler<AddProductionReportCommand, Result>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IProductionRepository _productionRepository;
        public AddProductionReportCommandHandler(IAppDbContext appDbContext, IProductionRepository productionRepository)
        {
            _appDbContext = appDbContext;
            _productionRepository = productionRepository;
        }

        public async Task<Result> Handle(AddProductionReportCommand request, CancellationToken cancellationToken)
        {
            // Lấy workshop hiện tại
            var workshopId = await _appDbContext.Users
                .Where(u => u.Id == request.StaffId)
                .Select(u => u.WorkshopId)
                .FirstOrDefaultAsync(cancellationToken);

            // Lấy assignment hiện tại
            var assignment = await _appDbContext.Assignments
                .Where(a => a.Id == request.AssignId)
                .FirstOrDefaultAsync(cancellationToken);

            if (assignment == null)
                throw new NotFoundException("Không tìm thấy Assignment.");

            // Lấy tất cả assignment của batch
            var assignmentsOfBatch = await _appDbContext.Assignments
                .Where(a => a.BatchId == assignment.BatchId)
                .ToListAsync(cancellationToken);

            //xử lý xưởng thường
            if (assignment.StepOrder.HasValue)
            {
                var workflowAssignments = assignmentsOfBatch
                    .Where(a => a.StepOrder.HasValue)
                    .ToList();

                // Xác định step đầu tiên của batch (KHÔNG cố định là 1 → đúng theo yêu cầu)
                int firstStepOrder = assignmentsOfBatch.Min(a => a.StepOrder!.Value);

                // Step hiện tại
                int currentStepOrder = assignment.StepOrder.Value;

                // Nếu đây là bước đầu → KHÔNG cần kiểm tra MaterialWorkshop
                bool isFirstWorkshop = currentStepOrder == firstStepOrder;

                if (!isFirstWorkshop)
                {
                    // Lấy assignment của xưởng trước
                    var previousAssignment = assignmentsOfBatch
                        .FirstOrDefault(a => a.StepOrder == currentStepOrder - 1);

                    if (previousAssignment == null)
                        throw new NotFoundException("Không tìm thấy xưởng trước trong quy trình.");

                    var currentWorkshopId = workshopId;

                    var previousMaterialStatus = await _appDbContext.MaterialWorkshops
                        .Where(mw =>
                            mw.AssignId == previousAssignment.Id &&
                            mw.WorkshopId == currentWorkshopId
                        )
                        .Select(mw => mw.Status)
                        .FirstOrDefaultAsync(cancellationToken);



                    if (previousMaterialStatus != "Confirmed")
                    {
                        throw new BadRequestException("Xưởng trước chưa chuyển hàng hoặc chưa QC Confirm. Không thể nộp báo cáo.");
                    }
                }
            }

            if (assignment.Status == "Reworking")
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.AssignmentId == assignment.Id && (r.Status == "InProgress" || r.Status == "Approved" || r.Status == "ReadyForTransfer")).FirstOrDefaultAsync(cancellationToken);
                var production = Production.Create(request.AssignId, request.StaffId, request.Quantity, reworkRequest?.Id);
                await _productionRepository.AddAsync(production);
            }
            else
            {
                var production = Production.Create(request.AssignId, request.StaffId, request.Quantity, null);
                await _productionRepository.AddAsync(production);
            }

            if (request.MaterialUsed.Count() > 0)
            {
                foreach (var items in request.MaterialUsed)
                {
                    if (items.QuantityUsed <= 0)
                        throw new BadRequestException("Số lượng sử dụng phải lớn hơn 0");

                    var listMaterialUse = await _appDbContext.MaterialUse.Where(m => m.MaterialId == items.MaterialId && m.AssignId == request.AssignId).ToListAsync();

                    if (listMaterialUse is null)
                    {
                        throw new NotFoundException("Không tìm thấy MaterailUse");
                    }

                    MaterialUse? targetMaterialUse;

                    if (assignment.Status == "Reworking")
                    {
                        targetMaterialUse = listMaterialUse.FirstOrDefault(m => m.ReworkRequestId != null);
                        if (targetMaterialUse == null)
                            throw new NotFoundException("Không tìm thấy MaterialUse thuộc ReworkRequest.");
                    }
                    else
                    {
                        targetMaterialUse = listMaterialUse.FirstOrDefault(m => m.ReworkRequestId == null);
                        if (targetMaterialUse == null)
                            throw new NotFoundException("Không tìm thấy MaterialUse của Assignment bình thường.");
                    }

                    targetMaterialUse.IncreaseQuantityStaffUse(items.QuantityUsed);
                }
            }

            return Result.Success();
        }
    }
}
