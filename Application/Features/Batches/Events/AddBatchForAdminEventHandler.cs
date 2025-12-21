using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Batches.Events
{
    public class AddBatchForAdminEventHandler : INotificationHandler<AddBatchForAdminEvent>
    {
        private readonly INotificationService _notificationService;

        public AddBatchForAdminEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(AddBatchForAdminEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendAddBatchForAdminNotificationAsync(notification.BatchCode, notification.Quantity);
        }
    }
}
