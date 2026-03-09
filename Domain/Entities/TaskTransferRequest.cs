using Domain.Primitives;

namespace Domain.Entities
{
    public class TaskTransferRequest : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public Guid QcTransportId { get; private set; }
        public Guid? MaterialRequestId { get; private set; }
        public Guid? AssignmentTransferId { get; private set; }
        public string Status { get; private set; }
        public string? Note { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime DateToGo { get; private set; }
        public DateTime? ApprovedAt { get; private set; }
        public DateTime? ReceivedAt { get; private set; }
        public TaskTransferRequest(Guid id, Guid batchId, Guid workshopId, Guid qcTransportId, Guid? materialRequestId, Guid? assignmentTransferId, string? note, DateTime dateToGo) : base(id)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            QcTransportId = qcTransportId;
            MaterialRequestId = materialRequestId;
            AssignmentTransferId = assignmentTransferId;
            Status = "Pending";
            Note = note;
            DateToGo = dateToGo;
            CreatedAt = DateTime.Now;
        }

        public static TaskTransferRequest Create(Guid batchId, Guid workshopId, Guid qcTransportId, Guid? requestId, Guid? assignmentTransferId, string? note, DateTime dateToGo)
        {
            return new TaskTransferRequest(Guid.NewGuid(), batchId, workshopId, qcTransportId, requestId, assignmentTransferId, note, dateToGo);
        }

        public void UpdateApproveStatus()
        {
            Status = "Approved";
            ApprovedAt = DateTime.Now;
        }

        public void MarkAsReceived()
        {
            ReceivedAt = DateTime.Now;
        }
    }
}
