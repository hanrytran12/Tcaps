using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialRequest : AggregrateRoot
    {
        public Guid MaterialId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid AssignId { get; private set; }
        public decimal QuantityRequest { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public DateOnly Date { get; private set; }
        public string Type { get; private set; }
        public string? NoteFromQC { get; private set; } = null;
        public decimal? ActualReceivedQuantity { get; private set; }

        public MaterialRequest(Guid id, Guid materialId, Guid userId, Guid batchId, Guid assignId, decimal quantityRequest, string note, string type)
            : base(id)
        {
            MaterialId = materialId;
            UserId = userId;
            BatchId = batchId;
            AssignId = assignId;
            QuantityRequest = quantityRequest;
            Status = "Pending";
            Note = note;
            Date = DateOnly.FromDateTime(DateTime.Now);
            Type = type;
        }

        public static MaterialRequest Create(Guid materialId, Guid userId, Guid batchId, Guid assignId, decimal quantityRequest, string note, string type)
        {
            return new MaterialRequest(Guid.NewGuid(), materialId, userId, batchId, assignId, quantityRequest, note, type);
        }

        public void MarkAsApproved()
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be approved.");

            Status = "Approved";
            AddDomainEvent(new MaterialRequestApprovedEvent(MaterialId, UserId, BatchId, QuantityRequest));
        }

        public void MarkAsConfirmed(decimal actualReceivedQuantity, string noteFromQC)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be confirmed.");

            Status = "Confirmed";
            ActualReceivedQuantity = actualReceivedQuantity;
            NoteFromQC = noteFromQC;
            AddDomainEvent(new MaterialRequestConfirmedEvent(MaterialId, BatchId, AssignId, QuantityRequest, actualReceivedQuantity));
        }

        public void MarkAsConfirmedWithDiscrepancy(decimal actualReceivedQuantity, string noteFromQC)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be confirmed.");

            Status = "ConfirmedWithDiscrepancy";
            ActualReceivedQuantity = actualReceivedQuantity;
            NoteFromQC = noteFromQC;
            AddDomainEvent(new MaterialRequestConfirmedEvent(MaterialId, BatchId, AssignId, QuantityRequest, actualReceivedQuantity));
        }

        public void MarkAdRejected(string rejectedReason)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be rejected.");

            Status = "Rejected";
            NoteFromQC = rejectedReason;
        }
    }
}
