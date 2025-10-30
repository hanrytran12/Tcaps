using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestConfirmedEventHandler : INotificationHandler<MaterialRequestConfirmedEvent>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMaterialUseRepository _materialUseRepository;

        public MaterialRequestConfirmedEventHandler(IMaterialUseRepository materialUseRepository, IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
            _materialUseRepository = materialUseRepository;
        }

        public async Task Handle(MaterialRequestConfirmedEvent notification, CancellationToken cancellationToken)
        {
            var materialUse = Domain.Entities.MaterialUse.Create(notification.MaterialId, notification.BatchId, notification.AssignId, notification.ActualReceivedQuantity);
            await _materialUseRepository.AddAsync(materialUse);

            var material = await _materialRepository.GetByIdAsync(notification.MaterialId);
            material?.DecreaseQuantity((int)notification.QuantityRequest);
        }
    }
}
