using MediatR;

namespace Domain.Events
{
    public class MaterialRequestApprovedEvent : INotification
    {
        public Guid MaterialId { get; }
        public Guid BatchId { get; }
        public decimal QuantityRequest { get; }

        public MaterialRequestApprovedEvent(Guid materialId, Guid batchId, decimal quantityRequest)
        {
            MaterialId = materialId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
        }

    }
}
