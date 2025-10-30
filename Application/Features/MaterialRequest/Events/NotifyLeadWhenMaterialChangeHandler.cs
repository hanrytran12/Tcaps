using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class NotifyLeadWhenMaterialChangeHandler : INotificationHandler<MaterialStockUpdatedEvent>
    {
        private readonly INotificationService _notificationsService;
        public NotifyLeadWhenMaterialChangeHandler(INotificationService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        public async Task Handle(MaterialStockUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationsService.SendStockUpdateNotificationToLeadAsync(notification.MaterialName, notification.NewStockQuantity, notification.QuantityChange);
        }
    }
}
