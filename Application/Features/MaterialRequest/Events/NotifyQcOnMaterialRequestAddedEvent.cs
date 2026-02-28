using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class NotifyQcOnMaterialRequestAddedEvent : INotificationHandler<MaterialRequestAddedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotifyQcOnMaterialRequestAddedEvent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MaterialRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendIncomingMaterialNotificationAsync(notification.Quantity, notification.AssignmentId, notification.MaterialName, notification.UnitMaterial);
        }
    }
}
