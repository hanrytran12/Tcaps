using MediatR;

namespace Domain.Events
{
    public class ReworkRequestAddedEvent : INotification
    {
        public Guid AssignmentId { get; set; }
        public Guid QcId { get; set; }
        public decimal DefectiveQuantity { get; set; }
        public string NoteQC { get; set; } = string.Empty;

        public ReworkRequestAddedEvent(Guid assignmentId, Guid qcId, decimal defectiveQuantity, string noteQC)
        {
            AssignmentId = assignmentId;
            QcId = qcId;
            DefectiveQuantity = defectiveQuantity;
            NoteQC = noteQC;
        }
    }
}
