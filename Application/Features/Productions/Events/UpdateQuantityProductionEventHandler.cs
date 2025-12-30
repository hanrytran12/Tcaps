using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Productions.Events
{
    public class UpdateQuantityProductionEventHandler : INotificationHandler<UpdateQuantityProductionEvent>
    {
        private readonly INotificationService _notificationService;

        public UpdateQuantityProductionEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(UpdateQuantityProductionEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendUpdateQuantityProductionNotificationAsync(
                notification.UserId,
                notification.QuantitySend,
                notification.QuantityReceive,
                notification.Date,
                notification.Time,
                notification.BatchCode);
        }
    }
}
