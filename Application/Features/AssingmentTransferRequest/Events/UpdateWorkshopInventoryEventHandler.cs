using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class UpdateWorkshopInventoryEventHandler : INotificationHandler<TransferRequestAddedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IWorkshopInventoryRepository _workshopInventoryRepository;

        public UpdateWorkshopInventoryEventHandler(IAppDbContext appDbContext, IWorkshopInventoryRepository workshopInventoryRepository)
        {
            _appDbContext = appDbContext;
            _workshopInventoryRepository = workshopInventoryRepository;
        }

        public async Task Handle(TransferRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            var materialUse = await _appDbContext.MaterialUse.Where(m => m.AssignId == notification.AssignmentId).ToListAsync();

            var assignment = await _appDbContext.Assignments.Where(a => a.Id == notification.AssignmentId).FirstOrDefaultAsync();

            if (assignment.Status == "Reworking")
            {
                materialUse = materialUse.Where(m => m.ReworkRequestId is not null).ToList();
            }

            foreach (var itemMaterialUse in materialUse)
            {
                var surplusQuantity = itemMaterialUse.QuantityDivide - itemMaterialUse.ReconciledQuantity;

                if (surplusQuantity > 0)
                {
                    var workshopInventory = await _appDbContext.WorkshopInventory.Where(w => w.MaterialId == itemMaterialUse.MaterialId && w.WorkshopId == assignment.WorkshopId).FirstOrDefaultAsync();
                    if (workshopInventory is not null)
                    {
                        workshopInventory.IncreaseQuantity(surplusQuantity);
                    }

                    else if (workshopInventory is null)
                    {
                        workshopInventory = Domain.Entities.WorkshopInventory.Create(assignment.WorkshopId, itemMaterialUse.MaterialId, surplusQuantity);
                        await _workshopInventoryRepository.AddAsync(workshopInventory);
                    }
                }
            }
        }
    }
}
