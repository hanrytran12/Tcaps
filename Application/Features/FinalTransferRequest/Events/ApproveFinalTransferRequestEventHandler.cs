using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.FinalTransferRequest.Events
{
    public class ApproveFinalTransferRequestEventHandler : INotificationHandler<ApproveFinalTransferRequestEvent>
    {
        private readonly INotificationService _notificationService;

        public ApproveFinalTransferRequestEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ApproveFinalTransferRequestEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.ApproveFinalTransferRequestNotificationAsync(
                notification.BatchId,
                notification.QuantityComplete,
                notification.QuantityError);
        }
    }
}
