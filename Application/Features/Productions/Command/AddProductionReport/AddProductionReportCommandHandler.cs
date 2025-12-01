using Application.Common;
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
            var assignment = await _appDbContext.Assignments.Where(a => a.Id == request.AssignId).FirstOrDefaultAsync(cancellationToken);

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (today < assignment.StartDate || today > assignment.EndDate)
            {
                return Result.Failure("Ngày nộp sản phẩm không nằm trong khoảng thời gian của Assignment.");
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
                        return Result.Failure("Số lượng sử dụng phải lớn hơn 0");

                    var listMaterialUse = await _appDbContext.MaterialUse.Where(m => m.MaterialId == items.MaterialId && m.AssignId == request.AssignId).ToListAsync();

                    if (listMaterialUse is null)
                    {
                        return Result.Failure("Không tìm thấy MaterailUse");
                    }

                    MaterialUse? targetMaterialUse;

                    if (assignment.Status == "Reworking")
                    {
                        targetMaterialUse = listMaterialUse.FirstOrDefault(m => m.ReworkRequestId != null);
                        if (targetMaterialUse == null)
                            return Result.Failure("Không tìm thấy MaterialUse thuộc ReworkRequest.");
                    }
                    else
                    {
                        targetMaterialUse = listMaterialUse.FirstOrDefault(m => m.ReworkRequestId == null);
                        if (targetMaterialUse == null)
                            return Result.Failure("Không tìm thấy MaterialUse của Assignment bình thường.");
                    }

                    //var quantityDivide = targetMaterialUse.QuantityDivide;     // Số lượng được chia
                    //var staffUsed = targetMaterialUse.QuantityStaffUse;        // Đã dùng trước đó
                    //var remaining = quantityDivide - staffUsed;                // Số còn lại có thể dùng

                    //if (items.QuantityUsed > quantityDivide)
                    //    return Result.Failure($"SL đưa vào ({items.QuantityUsed}) vượt SL được chia ({quantityDivide}).");

                    //if (items.QuantityUsed > remaining)
                    //    return Result.Failure(
                    //        $"SL sử dụng vượt mức cho phép. Đã dùng: {staffUsed}, " +
                    //        $"Được chia: {quantityDivide}, Còn lại: {remaining}, Bạn nhập: {items.QuantityUsed}"
                    //    );

                    targetMaterialUse.IncreaseQuantityStaffUse(items.QuantityUsed);
                }
            }

            return Result.Success();
        }
    }
}
