using MediatR;

namespace Domain.Events
{
    public class BatchCompletedEvent : INotification
    {
        public Guid BatchId { get; }
        public string BatchCode { get; }

        public BatchCompletedEvent(Guid batchId, string batchCode)
        {
            BatchId = batchId;
            BatchCode = batchCode;
        }
    }
}
