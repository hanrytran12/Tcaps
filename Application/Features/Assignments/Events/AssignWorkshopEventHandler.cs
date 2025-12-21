using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Assignments.Events
{
    public class AssignWorkshopEventHandler : INotificationHandler<AssignWorkshopEvent>
    {
        private readonly INotificationService _notificationService;

        public AssignWorkshopEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(AssignWorkshopEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.AssignWorkshopNotificationAsync(notification.UserId, notification.BatchCode);
        }
    }
}
