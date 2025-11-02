using MediatR;

namespace Domain.Events
{
    public class AssignmentAddedEvent : INotification
    {
        public string BatchCode { get; }
        public Guid WorkshopId { get; }
        public DateOnly? ExpectedDeliveryDate { get; }

        public AssignmentAddedEvent(string batchCode, Guid workshopId, DateOnly? expectedDeliveryDate)
        {
            BatchCode = batchCode;
            WorkshopId = workshopId;
            ExpectedDeliveryDate = expectedDeliveryDate;
        }
    }
}
