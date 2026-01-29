using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Assignments.Events
{
    public class AssignmentsPlannedEventHandler : INotificationHandler<AssignmentsPlannedEvent>
    {
        private readonly INotificationService _notificationService;

        public AssignmentsPlannedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(AssignmentsPlannedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendAssignmentsPlannedNotificationAsync(notification.Assignments, notification.BatchCode);
        }
    }
}
