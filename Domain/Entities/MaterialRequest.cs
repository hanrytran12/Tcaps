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
        public DateTime Date { get; private set; }
        public string Type { get; private set; }
        public string? NoteFromQC { get; private set; } = null;
        public decimal? ActualReceivedQuantity { get; private set; }
        public decimal QuantityFromStock { get; private set; }

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
            Date = DateTime.Now;
            Type = type;
        }

        private MaterialRequest() : base(Guid.NewGuid()) { }

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
            if (Status != "Pending" && Status != "QCTransportReception")
                throw new InvalidOperationException("Only pending requests can be confirmed.");

            Status = "Confirmed";
            ActualReceivedQuantity = actualReceivedQuantity;
            NoteFromQC = noteFromQC;
            AddDomainEvent(new MaterialRequestConfirmedEvent(MaterialId, BatchId, AssignId, QuantityRequest, actualReceivedQuantity));
            AddDomainEvent(new NotificationForStaffEvent(UserId, BatchId, ActualReceivedQuantity));
        }

        public void MarkAsConfirmedWithDiscrepancy(decimal actualReceivedQuantity, string noteFromQC)
        {
            if (Status != "Pending" && Status != "QCTransportReception")
                throw new InvalidOperationException("Only pending or QCTransportReception requests can be confirmed.");

            Status = "ConfirmedWithDiscrepancy";
            ActualReceivedQuantity = actualReceivedQuantity;
            NoteFromQC = noteFromQC;
            AddDomainEvent(new MaterialRequestConfirmedEvent(MaterialId, BatchId, AssignId, QuantityRequest, actualReceivedQuantity));
        }

        public void MarkAdRejected(string rejectedReason)
        {
            if (Status != "Pending" && Status != "QCTransportReception")
                throw new InvalidOperationException("Only pending or QCTransportReception requests can be confirmed.");

            Status = "Rejected";
            NoteFromQC = rejectedReason;
        }

        public void MarkAsReception() => Status = "QCTransportReception";
        public void MarkAsConfirmFromLead() => Status = "Confirmed";

        public void IncreaseQuantityFromStock(decimal quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity to increase must be non-negative.", nameof(quantity));
            QuantityFromStock += quantity;
        }

        public void IncreaseQuantityActual(decimal quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity to increase must be non-negative.", nameof(quantity));
            ActualReceivedQuantity += quantity;
        }
    }
}
