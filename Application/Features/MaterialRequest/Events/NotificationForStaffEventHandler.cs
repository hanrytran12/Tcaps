using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class NotificationForStaffEventHandler : INotificationHandler<NotificationForStaffEvent>
    {
        private readonly INotificationService _notificationService;

        public NotificationForStaffEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(NotificationForStaffEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendNotificationForStaffNotificationAsync(
                notification.UserId,
                notification.BatchId,
                notification.ActualReceivedQuantity);
        }
    }
}
