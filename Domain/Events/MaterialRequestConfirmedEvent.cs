using MediatR;

namespace Domain.Events
{
    public class MaterialRequestConfirmedEvent : INotification
    {
        public Guid MaterialId { get; set; }
        public Guid BatchId { get; set; }
        public decimal QuantityRequest { get; set; }

        public MaterialRequestConfirmedEvent(Guid materialId, Guid batchId, decimal quantityRequest)
        {
            MaterialId = materialId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
        }
    }
}
