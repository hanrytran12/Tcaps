using MediatR;

namespace Domain.Events
{
    public class MaterialRequestConfirmedEvent : INotification
    {
        public Guid MaterialId { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityRequest { get; set; }

        public MaterialRequestConfirmedEvent(Guid materialId, Guid batchId, Guid assignId, decimal quantityRequest)
        {
            MaterialId = materialId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
            AssignId = assignId;
        }
    }
}
