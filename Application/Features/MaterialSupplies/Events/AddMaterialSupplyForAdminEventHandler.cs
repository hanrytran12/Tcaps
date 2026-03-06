using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialSupplies.Events
{
    public class AddMaterialSupplyForAdminEventHandler : INotificationHandler<AddMaterialSupplyForAdminEvent>
    {
        private readonly INotificationService _notificationService;

        public AddMaterialSupplyForAdminEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(AddMaterialSupplyForAdminEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendAddMaterialSupplyForAdminNotification(
                notification.MaterialId,
                notification.QuantitySend,
                notification.DateShip);
        }
    }
}
