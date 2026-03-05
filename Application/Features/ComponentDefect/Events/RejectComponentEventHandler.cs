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
    public class RejectComponentEventHandler : INotificationHandler<RejectComponentEvent>
    {
        private readonly INotificationService _notificationService;

        public RejectComponentEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(RejectComponentEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.RejectComponentDefectNotificationAsync(
                notification.ComponentId,
                notification.EvaluateId,
                notification.Quantity,
                notification.QuantityReject);
        }
    }
}
