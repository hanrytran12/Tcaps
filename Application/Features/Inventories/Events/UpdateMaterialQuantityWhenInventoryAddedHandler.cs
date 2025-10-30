using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Events
{
    public class UpdateMaterialQuantityWhenInventoryAddedHandler : INotificationHandler<InventoryAddEvent>
    {
        private readonly IMaterialRepository _materialRepository;

        public UpdateMaterialQuantityWhenInventoryAddedHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task Handle(InventoryAddEvent notification, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetByIdAsync(notification.MaterialId);
            if (material is not null)
            {
                material.IncreasePrice(notification.Price);
                material.IncreaseQuantity(notification.AddedQuantity);
            }
        }
    }
}
