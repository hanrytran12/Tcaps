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
    public class UpdateQuantityDefectiveEventHandler : INotificationHandler<UpdateQuantityDefectiveEvent>
    {
        private readonly INotificationService _notificationService;

        public UpdateQuantityDefectiveEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(UpdateQuantityDefectiveEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.UpdateQuantityDefectNotificationAsync(notification.QuantityReject, notification.QcId, notification.BatchCode);
        }
    }
}
