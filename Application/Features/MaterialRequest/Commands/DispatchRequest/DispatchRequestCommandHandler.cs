using Application.Common;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Commands.DispatchRequest
{
    public class DispatchRequestCommandHandler : IRequestHandler<DispatchRequestCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IAppDbContext _appDbContext;
        private readonly IWorkshopInventoryRepository _workshopInventoryRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepository;

        public DispatchRequestCommandHandler(IBatchRepository batchRepository, IMaterialRequestRepository materialRequestRepository, IAppDbContext appDbContext, IWorkshopInventoryRepository workshopInventoryRepository, IAssignmentRepository assignmentRepository, 
            IUserRepository userRepository)
        {
            _batchRepository = batchRepository;
            _materialRequestRepository = materialRequestRepository;
            _appDbContext = appDbContext;
            _workshopInventoryRepository = workshopInventoryRepository;
            _assignmentRepository = assignmentRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DispatchRequestCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByAssignmentIdAsync(request.AssignmentId);

            if (batch is null)
            {
                return Result.Failure("Không tìm thấy lô hàng");
            }

            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);

            var qc = await _userRepository.GetQCByWorkshopIdAsync(assignment.WorkshopId);

            foreach (var item in request.Items)
            {
                var materialRequest = Domain.Entities.MaterialRequest.Create(item.MaterialId, qc.Id, batch.Id, request.AssignmentId, item.Quantity, "Đơn xuất kho từ Lead", request.Type);
                await _materialRequestRepository.AddAsync(materialRequest);

                var material = await _appDbContext.Materials.Where(m => m.Id == item.MaterialId).FirstOrDefaultAsync();
                materialRequest.AddDomainEvent(new MaterialRequestAddedEvent(item.Quantity, request.AssignmentId, material.Name, material.Unit));

                var workshopInventory = await _workshopInventoryRepository.GetByMaterialIdAndWorkshopIdAsync(item.MaterialId, assignment.WorkshopId);
                if (workshopInventory is not null)
                {
                    workshopInventory.HoldStock();
                    materialRequest.IncreaseQuantityFromStock(workshopInventory.HoldingQuantity);
                }
            }

            return Result.Success();
        }
    }
}
