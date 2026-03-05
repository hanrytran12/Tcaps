using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Productions.Events
{
    public class MaterialShortageNotifiedEventHandler : INotificationHandler<MaterialShortageNotifiedEvent>
    {
        private readonly INotificationService _notificationService;

        public MaterialShortageNotifiedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MaterialShortageNotifiedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMaterialShortageNotificationAsync(
                notification.AssignId,
                notification.StaffId,
                notification.MaterialId,
                notification.MaterialName,
                notification.MaterialUnit,
                notification.QuantityRemaining,
                notification.WorkshopId);
        }
    }
}
