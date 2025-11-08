using MediatR;

namespace Domain.Events
{
    public class TransferRequestAddedEvent : INotification
    {
        public Guid UserId { get; set; }
        public Guid AssignmentId { get; set; }

        public TransferRequestAddedEvent(Guid userId, Guid assignmentId)
        {
            UserId = userId;
            AssignmentId = assignmentId;
        }
    }
}
