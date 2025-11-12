using MediatR;

namespace Domain.Events
{
    public class ReworkRequestCompletedEvent : INotification
    {
        public Guid ReworkRequestId { get; set; }

        public ReworkRequestCompletedEvent(Guid reworkRequestId)
        {
            ReworkRequestId = reworkRequestId;
        }
    }
}
