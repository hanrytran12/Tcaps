using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Events
{
    public class NotifyQcOnReworkRequestApprovedHandler : INotificationHandler<ReworkRequestApprovedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotifyQcOnReworkRequestApprovedHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(ReworkRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.QCOnReworkRequestApproveNotificationAsync(
                notification.QcId,
                notification.DeliveryDate,
                notification.EndDate);
        }
    }
}
