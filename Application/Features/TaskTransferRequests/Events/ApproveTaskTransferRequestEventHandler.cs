using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.TaskTransferRequests.Events
{
    public class ApproveTaskTransferRequestEventHandler : INotificationHandler<ApproveTaskTransferRequestEvent>
    {
        private readonly INotificationService _notificationService;

        public ApproveTaskTransferRequestEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ApproveTaskTransferRequestEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendApproveTaskTransferRequestNotificationAsync(notification.TaskTransferRequestId, notification.QCTransportId);
        }
    }
}
