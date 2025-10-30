using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialUse.Events
{
    public class UpdateMaterialQuantityWhenMaterialUseAddedEventHandler : INotificationHandler<MaterialUseAddedEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public UpdateMaterialQuantityWhenMaterialUseAddedEventHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task Handle(MaterialUseAddedEvent notification, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetByIdAsync(notification.MaterialId);
            if (materials is not null)
            {
                materials.DecreaseQuantity((int)notification.QuantityDivide);
            }
        }
    }
}
