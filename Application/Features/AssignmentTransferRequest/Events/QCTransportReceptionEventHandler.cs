using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AssignmentTransferRequest.Events
{
    public class QCTransportReceptionEventHandler : INotificationHandler<QCTransportReceptionAssignmentTransferEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<QCTransportReceptionEventHandler> _logger;

        public QCTransportReceptionEventHandler(INotificationService notificationService, ILogger<QCTransportReceptionEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }
        public async Task Handle(QCTransportReceptionAssignmentTransferEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendQCTransportReceptionAssignmentTransferNotificationAsync(notification.QcTransportId, notification.AssignmentTransferRequestId, notification.AssignId);

            //_ = Task.Run(async () =>
            //{
            //    try
            //    {
            //        await _notificationService.SendQCTransportReceptionAssignmentTransferNotificationAsync(
            //            notification.QcTransportId,
            //            notification.AssignmentTransferRequestId,
            //            notification.AssignId);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Lỗi gửi notification cho AssignmentTransfer {Id}", notification.AssignmentTransferRequestId);
            //    }
            //}, cancellationToken);
            //return Task.CompletedTask;
        }
    }
}
