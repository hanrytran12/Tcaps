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
        public string? RejectionReason { get; private set; } = null;

        public MaterialRequest(Guid id, Guid materialId, Guid userId, Guid batchId, Guid assignId, decimal quantityRequest, string note)
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
        }

        private MaterialRequest() : base(Guid.NewGuid()) { }

        public static MaterialRequest Create(Guid materialId, Guid userId, Guid batchId, Guid assignId, decimal quantityRequest, string note)
        {
            return new MaterialRequest(Guid.NewGuid(), materialId, userId, batchId, assignId, quantityRequest, note);
        }

        public void MarkAsApproved()
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be approved.");

            Status = "Approved";
            AddDomainEvent(new MaterialRequestApprovedEvent(MaterialId, BatchId, QuantityRequest));
        }

        public void MarkAsConfirmed()
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be confirmed.");

            Status = "Confirmed";
            AddDomainEvent(new MaterialRequestConfirmedEvent(MaterialId, BatchId, AssignId, QuantityRequest));
        }

        public void MarkAdRejected(string rejectedReason)
        {
            if (Status != "Pending")
                throw new InvalidOperationException("Only pending requests can be rejected.");

            Status = "Rejected";
            RejectionReason = rejectedReason;
        }
    }
}
