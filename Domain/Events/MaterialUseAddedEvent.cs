using MediatR;

namespace Domain.Events
{
    public class MaterialUseAddedEvent : INotification
    {
        public Guid MaterialId { get; }
        public Guid BatchId { get; }
        public Guid AssignId { get; }
        public decimal QuantityDivide { get; }

        public MaterialUseAddedEvent(Guid materialId, Guid batchId, Guid assignId, decimal quantityDivide)
        {
            MaterialId = materialId;
            BatchId = batchId;
            AssignId = assignId;
            QuantityDivide = quantityDivide;
        }
    }
}
