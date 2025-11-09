using Domain.Primitives;

namespace Domain.Entities
{
    public class ReworkRequest : Entity
    {
        public Guid QcId { get; private set; }
        public Guid AssignmentId { get; private set; }
        public decimal DefectiveQuantity { get; private set; }
        public string NoteQc { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public ReworkRequest(Guid id, Guid qcId, Guid assignmentId, decimal defectiveQuantity, string noteQc)
            : base(id)
        {
            QcId = qcId;
            AssignmentId = assignmentId;
            DefectiveQuantity = defectiveQuantity;
            NoteQc = noteQc;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = "PendingLead";
        }

        private ReworkRequest() : base(Guid.NewGuid()) { }

        public static ReworkRequest Create(Guid qcId, Guid assignmentId, decimal defectiveQuantity, string noteQc)
        {
            return new ReworkRequest(Guid.NewGuid(), qcId, assignmentId, defectiveQuantity, noteQc);
        }
    }
}
