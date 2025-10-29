using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestConfirmedEventHandler : INotificationHandler<MaterialRequestConfirmedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;

        public MaterialRequestConfirmedEventHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task Handle(MaterialRequestConfirmedEvent notification, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(notification.BatchId);
            var materialUse = Domain.Entities.MaterialUse.Create(notification.MaterialId, notification.BatchId, notification.AssignId, notification.ActualReceivedQuantity);
            batch.AddMaterialUse(materialUse, notification.QuantityRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
