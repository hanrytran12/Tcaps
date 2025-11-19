using MediatR;

namespace Domain.Events
{
    public class AssignmentsPlannedEvent : INotification
    {
        public string BatchCode { get; }
        public List<AssignmentsInfo> Assignments { get; }

        public AssignmentsPlannedEvent(string batchCode, List<AssignmentsInfo> assignments)
        {
            BatchCode = batchCode;
            Assignments = assignments;
        }
    }

    public class AssignmentsInfo
    {
        public Guid WorkshopId { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
    }
}
