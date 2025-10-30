using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.ComponentDefect.Events
{
    public class ComponentConfirmEventHandler : INotificationHandler<ComponentConfirmEvent>
    {
        private readonly INotificationService _notificationService;

        public ComponentConfirmEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ComponentConfirmEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendComponentConfirmNotification(notification.Id, notification.EvaluateId, notification.Quantity, notification.Status);
        }
    }
}
