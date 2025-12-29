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
    public class FinalTransferRequestForGuardQCEventHandler : INotificationHandler<FinalTransferRequestForGuardQCEvent>
    {
        private readonly INotificationService _notificationService;

        public FinalTransferRequestForGuardQCEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(FinalTransferRequestForGuardQCEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendFinalTransferRequestForGuardQCNotificationAsync(
                notification.QuantitySend,
                notification.BatchCode,
                notification.WorkshopName);
        }
    }
}
