using MediatR;

namespace Domain.Events
{
    public class TransferRequestAddedEvent : INotification
    {
        public Guid UserId { get; set; }

        public TransferRequestAddedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
