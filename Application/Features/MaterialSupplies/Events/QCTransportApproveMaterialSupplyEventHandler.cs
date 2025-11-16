using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialSupplies.Events
{
    public class QCTransportApproveMaterialSupplyEventHandler : INotificationHandler<QCTransportApproveMaterialSupplyEvent>
    {
        private readonly INotificationService _notificationService;

        public QCTransportApproveMaterialSupplyEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(QCTransportApproveMaterialSupplyEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendQCTransportApproveMaterialSupplyNotificationAsync(
                notification.QcTransportId,
                notification.MaterialSupplyId);
        }
    }
}
