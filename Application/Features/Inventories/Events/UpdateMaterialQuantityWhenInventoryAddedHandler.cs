using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Events
{
    public class UpdateMaterialQuantityWhenInventoryAddedHandler : INotificationHandler<InventoryAddEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateMaterialQuantityWhenInventoryAddedHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task Handle(InventoryAddEvent notification, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetByIdAsync(notification.MaterialId);
            if (material is not null)
            {
                material.IncreaseQuantity(notification.AddedQuantity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                foreach (var domainEvent in material.DomainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
                material.ClearDomainEvent();
            }
        }
    }
}
