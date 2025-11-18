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
    public class QcTransportReceptionMaterialRequestEventHandler : INotificationHandler<QcTransportReceptionMaterialRequestEvent>
    {
        private readonly INotificationService _notificationService;

        public QcTransportReceptionMaterialRequestEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(QcTransportReceptionMaterialRequestEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendQCTransportReceptionMaterialRequestNotificationAsync(
                notification.QcTransportId,
                notification.MaterialRequestId,
                notification.AssignId);
        }
    }
}
