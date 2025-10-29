using MediatR;

namespace Domain.Events
{
    public class MaterialRequestConfirmedEvent : INotification
    {
        public Guid MaterialId { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityRequest { get; set; }
        public decimal ActualReceivedQuantity { get; set; }

        public MaterialRequestConfirmedEvent(Guid materialId, Guid batchId, Guid assignId, decimal quantityRequest, decimal actualReceivedQuantity)
        {
            MaterialId = materialId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
            AssignId = assignId;
            ActualReceivedQuantity = actualReceivedQuantity;
        }
    }
}
