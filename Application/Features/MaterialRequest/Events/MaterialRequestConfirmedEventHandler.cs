using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestConfirmedEventHandler : INotificationHandler<MaterialRequestConfirmedEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMaterialUseRepository _materialUseRepository;
        private readonly IWorkshopInventoryRepository _workshopInventoryRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAppDbContext _appDbContext;

        public MaterialRequestConfirmedEventHandler(IMaterialUseRepository materialUseRepository, IMaterialRepository materialRepository, IWorkshopInventoryRepository workshopInventoryRepository, IAssignmentRepository assignmentRepository, IAppDbContext appDbContext)
        {
            _materialRepository = materialRepository;
            _materialUseRepository = materialUseRepository;
            _workshopInventoryRepository = workshopInventoryRepository;
            _assignmentRepository = assignmentRepository;
            _appDbContext = appDbContext;
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

            if (assignment.Status != "Reworking")
            {
                var materialUse = Domain.Entities.MaterialUse.Create(notification.MaterialId, notification.BatchId, notification.AssignId, notification.ActualReceivedQuantity, null);
                await _materialUseRepository.AddAsync(materialUse);

            }
            else
            {
                var reworkRequest = await _appDbContext.ReworkRequests.AsNoTracking().Where(r => r.AssignmentId == assignment.Id).SingleOrDefaultAsync();
                var materialUse = Domain.Entities.MaterialUse.Create(notification.MaterialId, notification.BatchId, notification.AssignId, notification.ActualReceivedQuantity, reworkRequest.Id);
                await _materialUseRepository.AddAsync(materialUse);
            }

            var batch = await _appDbContext.Batches
                .Where(b => b.Id == notification.BatchId)
                .FirstOrDefaultAsync();

            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng.");
            }

            var material = await _materialRepository.GetByIdAsync(notification.MaterialId);
            material?.DecreaseQuantity((int)notification.QuantityRequest, batch.UserId ?? Guid.Empty);
        }
    }
}
