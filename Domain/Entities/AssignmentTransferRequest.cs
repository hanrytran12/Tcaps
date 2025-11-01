using Domain.Primitives;

namespace Domain.Entities
{
    public class AssignmentTransferRequest : Entity
    {
        public Guid AssignmentId { get; private set; }
        public Guid UserId { get; private set; }
        public decimal CompletedQuantity { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public AssignmentTransferRequest(Guid RequestId, Guid assignmentId, Guid userId, decimal completedQuantity, string status, string note)
            : base(RequestId)
        {
            AssignmentId = assignmentId;
            UserId = userId;
            CompletedQuantity = completedQuantity;
            Status = status;
            Note = note;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        private AssignmentTransferRequest() : base(Guid.NewGuid()) { }

        public static AssignmentTransferRequest Create(Guid RequestId, Guid assignmentId, Guid userId, decimal completedQuantity, string status, string note)
        {
            return new AssignmentTransferRequest(Guid.NewGuid(), assignmentId, userId, completedQuantity, status, note);
        }
    }
}
