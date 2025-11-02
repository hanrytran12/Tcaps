using MediatR;

namespace Domain.Events
{
    public class AssignmentActivedEvent : INotification
    {
        public string CodeBatch { get; set; }
        public Guid WorkshopId { get; set; }
        public DateOnly NextStartDate { get; set; }
        public Guid WorkshopIdNext { get; set; }

        public AssignmentActivedEvent(string codeBatch, Guid workshopId, DateOnly nextStartDate, Guid workshopIdNext)
        {
            CodeBatch = codeBatch;
            WorkshopId = workshopId;
            NextStartDate = nextStartDate;
            WorkshopIdNext = workshopIdNext;
        }
    }
}
