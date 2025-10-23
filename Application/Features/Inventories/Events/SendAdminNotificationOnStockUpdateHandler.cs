using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Inventories.Events
{
    public class SendAdminNotificationOnStockUpdateHandler : INotificationHandler<MaterialStockUpdatedEvent>
    {
        private readonly INotificationService _notificationService;
        public SendAdminNotificationOnStockUpdateHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MaterialStockUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendStockUpdateNotificationToAdminAsync(notification.MaterialName, notification.NewStockQuantity, notification.QuantityChange);
        }
    }
}
