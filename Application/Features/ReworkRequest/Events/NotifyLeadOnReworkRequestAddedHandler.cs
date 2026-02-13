using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Events
{
    public class NotifyLeadOnReworkRequestAddedHandler : INotificationHandler<ReworkRequestAddedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotifyLeadOnReworkRequestAddedHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(ReworkRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.LeadOnReworkRequestAddedNotificationAsync(
                notification.AssignmentId,
                notification.QcId,
                notification.DefectiveQuantity,
                notification.NoteQC);
        }
    }
}
