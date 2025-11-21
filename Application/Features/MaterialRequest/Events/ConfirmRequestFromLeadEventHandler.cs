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
    public class ConfirmRequestFromLeadEventHandler : INotificationHandler<ConfirmRequestFromLeadEvent>
    {
        private readonly INotificationService _notificationService;

        public ConfirmRequestFromLeadEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ConfirmRequestFromLeadEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendConfirmRequestFromLeadNotificationAsync(
                notification.MaterialRequestId, 
                notification.QcId);
        }
    }
}
