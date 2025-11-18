using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class AssignmentTransferRequest : AggregrateRoot
    {
        public Guid AssignmentId { get; private set; }
        public Guid? ReworkRequestId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal CompletedQuantity { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Note { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public AssignmentTransferRequest(Guid RequestId, Guid assignmentId, Guid userId, decimal completedQuantity, string? note, Guid? reworkRequestId)
            : base(RequestId)
        {
            AssignmentId = assignmentId;
            UserId = userId;
            CompletedQuantity = completedQuantity;
            Status = "PendingApproval";
            Note = note;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            ReworkRequestId = reworkRequestId;
        }

        private AssignmentTransferRequest() : base(Guid.NewGuid()) { }

        public static AssignmentTransferRequest Create(Guid assignmentId, Guid userId, decimal completedQuantity, string? note, Guid? reworkRequestId)
        {
            if (assignmentId == Guid.Empty) throw new ArgumentException("AssignmentId không được để trống.");
            if (userId == Guid.Empty) throw new ArgumentException("UserId không được để trống.");
            if (completedQuantity < 0) throw new ArgumentException("Quantity không được là số âm.");

            var transferRequest = new AssignmentTransferRequest(Guid.NewGuid(), assignmentId, userId, completedQuantity, note, reworkRequestId);
            //transferRequest.AddDomainEvent(new TransferRequestAddedEvent(userId));
            return transferRequest;
        }

        public void MarkAsApproved()
        {
            if (Status == "Approved") return;
            this.Status = "Approved";

            AddDomainEvent(new TransferRequestApprovedEvent(AssignmentId, ReworkRequestId, CompletedQuantity));
        }

        public void MarkAsReception() => Status = "QCTransportReception";
    }
}
