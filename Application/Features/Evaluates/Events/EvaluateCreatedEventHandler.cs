using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Events
{
    public class EvaluateCreatedEventHandler : INotificationHandler<EvaluateCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public EvaluateCreatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(EvaluateCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendEvaluateFixErrorNotificationAsync(notification.EvaluateId, notification.ProductionId, notification.UserId.Value, notification.QuantityError, notification.Note);
        }
    }
}
