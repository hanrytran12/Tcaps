using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialUse.Events
{
    public class UpdateMaterialQuantityWhenMaterialUseAddedEventHandler : INotificationHandler<MaterialUseAddedEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMaterialQuantityWhenMaterialUseAddedEventHandler(IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _materialRepository = materialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MaterialUseAddedEvent notification, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetByIdAsync(notification.MaterialId);
            if (materials is not null)
            {
                materials.DecreaseQuantity((int)notification.QuantityDivide);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
