using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class ReworkRequest : AggregrateRoot
    {
        public Guid QcId { get; private set; }
        public Guid AssignmentId { get; private set; }
        public decimal DefectiveQuantity { get; private set; }
        public string NoteQc { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateOnly? DeliveryDate { get; private set; }
        public DateOnly? EndDate { get; private set; }
        public DateOnly? NextStepDeliveryDate { get; private set; }

        public ReworkRequest(Guid id, Guid qcId, Guid assignmentId, decimal defectiveQuantity, string noteQc)
            : base(id)
        {
            QcId = qcId;
            AssignmentId = assignmentId;
            DefectiveQuantity = defectiveQuantity;
            NoteQc = noteQc;
            CreatedAt = DateTime.Now;
            Status = "PendingLead";
        }

        private ReworkRequest() : base(Guid.NewGuid()) { }

        public static ReworkRequest Create(Guid qcId, Guid assignmentId, decimal defectiveQuantity, string noteQc)
        {
            var reworkRequest = new ReworkRequest(Guid.NewGuid(), qcId, assignmentId, defectiveQuantity, noteQc);
            reworkRequest.AddDomainEvent(new ReworkRequestAddedEvent(assignmentId, qcId, defectiveQuantity, noteQc));
            return reworkRequest;
        }

        public void RejecetedRequest()
        {
            Status = "Rejected";
        }

        public void ApproveRequest(DateOnly deliveryDate, DateOnly endDate, DateOnly nextStepDeliveryDate)
        {
            Status = "Approved";
            DeliveryDate = deliveryDate;
            EndDate = endDate;
            NextStepDeliveryDate = nextStepDeliveryDate;
        }

        public void InProgressRequest()
        {
            Status = "InProgress";
        }

        public void Active()
        {
            Status = "ReadyForTransfer";
        }

        public void Completed()
        {
            Status = "Completed";
        }
    }
}
