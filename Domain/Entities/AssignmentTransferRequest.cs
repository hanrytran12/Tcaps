using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class AssignmentTransferRequest : AggregrateRoot
    {
        public Guid AssignmentId { get; private set; }
        public Guid? ReworkRequestId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal CompletedQuantitySend { get; private set; }
        public decimal CompletedQuantityReceive { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Note { get; private set; } = string.Empty;
        public string? NoteLead { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        public AssignmentTransferRequest(Guid RequestId, Guid assignmentId, Guid userId, decimal completedQuantitySend, string? note, Guid? reworkRequestId)
            : base(RequestId)
        {
            AssignmentId = assignmentId;
            UserId = userId;
            CompletedQuantitySend = completedQuantitySend;
            Status = "PendingApproval";
            Note = note;
            CreatedAt = DateTime.Now;
            ReworkRequestId = reworkRequestId;
        }

        private AssignmentTransferRequest() : base(Guid.NewGuid()) { }

        public static AssignmentTransferRequest Create(Guid assignmentId, Guid userId, decimal completedQuantitySend, string? note, Guid? reworkRequestId)
        {
            if (assignmentId == Guid.Empty) throw new ArgumentException("AssignmentId không được để trống.");
            if (userId == Guid.Empty) throw new ArgumentException("UserId không được để trống.");
            if (completedQuantitySend < 0) throw new ArgumentException("Quantity không được là số âm.");

            var transferRequest = new AssignmentTransferRequest(Guid.NewGuid(), assignmentId, userId, completedQuantitySend, note, reworkRequestId);
            //transferRequest.AddDomainEvent(new TransferRequestAddedEvent(userId));
            return transferRequest;
        }

        public void MarkAsInProgress(Guid supplierId, Guid assignmentTransferRequestId, decimal completedQuantityReceive, string notLead)
        {
            if (Status == "InProgress") return;
            this.Status = "InProgress";
            this.CompletedQuantityReceive = completedQuantityReceive;
            this.NoteLead = notLead;

            decimal quantityReject = CompletedQuantitySend - CompletedQuantityReceive;
            AddDomainEvent(new TransferRequestInProgressEvent(AssignmentId, assignmentTransferRequestId, ReworkRequestId, CompletedQuantityReceive, quantityReject, supplierId));
        }

        public void MarkAsApproved() => Status = "Approved";

        public void MarkAsReception() => Status = "QCTransportReception";
    }
}
