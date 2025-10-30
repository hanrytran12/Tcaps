using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Inventories.Events
{
    public class NotifyStakeholderOnStockUpdateHandler : INotificationHandler<MaterialStockUpdatedEvent>
    {
        private readonly INotificationService _notificationService;
        public NotifyStakeholderOnStockUpdateHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MaterialStockUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.CreateStockUpdateNotificationForRoleAsync("Admin", notification.MaterialName, notification.NewStockQuantity, notification.QuantityChange);
            await _notificationService.CreateStockUpdateNotificationForRoleAsync("Lead", notification.MaterialName, notification.NewStockQuantity, notification.QuantityChange);
        }
    }
}
