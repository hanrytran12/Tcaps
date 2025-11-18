using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class QCTransportReceptionEventHandler : INotificationHandler<QCTransportReceptionAssignmentTransferEvent>
    {
        private readonly INotificationService _notificationService;

        public QCTransportReceptionEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(QCTransportReceptionAssignmentTransferEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendQCTransportReceptionAssignmentTransferNotificationAsync(notification.QcTransportId, notification.AssignmentTransferRequestId, notification.AssignId);
        }
    }
}
