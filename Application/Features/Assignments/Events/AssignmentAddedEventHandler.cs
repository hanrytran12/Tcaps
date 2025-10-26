using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Assignments.Events
{
    public class AssignmentAddedEventHandler : INotificationHandler<AssignmentAddedEvent>
    {
        private readonly INotificationService _notification;
        public AssignmentAddedEventHandler(INotificationService notification)
        {
            _notification = notification;
        }

        public async Task Handle(AssignmentAddedEvent notification, CancellationToken cancellationToken)
        {
            await _notification.SendAssignmentAddNotificationToQcAsync(notification.BatchCode, notification.WorkshopId, notification.ExpectedDeliveryDate);
        }
    }
}
