using MediatR;

namespace Domain.Events
{
    public class AssignmentAddedEvent : INotification
    {
        public string BatchCode { get; }
        public Guid WorkshopId { get; }

        public AssignmentAddedEvent(string batchCode, Guid workshopId)
        {
            BatchCode = batchCode;
            WorkshopId = workshopId;
        }
    }
}
