using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestConfirmedEventHandler : INotificationHandler<MaterialRequestConfirmedEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMaterialUseRepository _materialUseRepository;
        private readonly IWorkshopInventoryRepository _workshopInventoryRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public MaterialRequestConfirmedEventHandler(IMaterialUseRepository materialUseRepository, IMaterialRepository materialRepository, IWorkshopInventoryRepository workshopInventoryRepository, IAssignmentRepository assignmentRepository)
        {
            _materialRepository = materialRepository;
            _materialUseRepository = materialUseRepository;
            _workshopInventoryRepository = workshopInventoryRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task Handle(MaterialRequestConfirmedEvent notification, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(notification.AssignId);

            var workshopInventory = await _workshopInventoryRepository.GetByMaterialIdAndWorkshopIdAsync(notification.MaterialId, assignment.WorkshopId);

            if (workshopInventory is not null)
            {
                notification.ActualReceivedQuantity += workshopInventory.Quantity;
                workshopInventory.DecreaseQuantity(workshopInventory.Quantity);
            }

            var materialUse = Domain.Entities.MaterialUse.Create(notification.MaterialId, notification.BatchId, notification.AssignId, notification.ActualReceivedQuantity);
            await _materialUseRepository.AddAsync(materialUse);

            var material = await _materialRepository.GetByIdAsync(notification.MaterialId);
            material?.DecreaseQuantity((int)notification.QuantityRequest);
        }
    }
}
