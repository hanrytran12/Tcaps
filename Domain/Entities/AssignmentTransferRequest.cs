using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class AssignmentTransferRequest : AggregrateRoot
    {
        public Guid AssignmentId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal CompletedQuantity { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Note { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public AssignmentTransferRequest(Guid RequestId, Guid assignmentId, Guid userId, decimal completedQuantity, string? note)
            : base(RequestId)
        {
            AssignmentId = assignmentId;
            UserId = userId;
            CompletedQuantity = completedQuantity;
            Status = "PendingLead";
            Note = note;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        private AssignmentTransferRequest() : base(Guid.NewGuid()) { }

        public static AssignmentTransferRequest Create(Guid assignmentId, Guid userId, decimal completedQuantity, string? note)
        {
            var transferRequest = new AssignmentTransferRequest(Guid.NewGuid(), assignmentId, userId, completedQuantity, note);
            transferRequest.AddDomainEvent(new TransferRequestAddedEvent(userId));
            return transferRequest;
        }
    }
}
