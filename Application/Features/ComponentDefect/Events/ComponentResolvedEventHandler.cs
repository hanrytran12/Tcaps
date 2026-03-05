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
    public class ComponentResolvedEventHandler : INotificationHandler<ComponentResolvedEvent>
    {
        private readonly INotificationService _notificationService;

        public ComponentResolvedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ComponentResolvedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.WasRejected)
            {
                // Staff đã sửa xong lỗi Reject -> thông báo cho QC vào confirm lại
                await _notificationService.SendComponentRejectResolvedNotificationAsync(
                    notification.Id,
                    notification.EvaluateId,
                    notification.Quantity,
                    notification.QuantityReject);
            }
            else
            {
                await _notificationService.SendComponentResolvedNotification(
                    notification.Id,
                    notification.EvaluateId,
                    notification.Quantity,
                    notification.Status);
            }
        }
    }
}
