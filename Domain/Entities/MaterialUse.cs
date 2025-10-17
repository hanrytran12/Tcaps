using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialUse : Entity
    {
        public Guid MaterialId { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid AssignId { get; private set; }
        public decimal QuantityUsed { get; private set; }
        public decimal QuantityRemaining { get; private set; }
        public DateOnly Date { get; private set; }

        public MaterialUse(Guid id, Guid materialId, Guid batchId, Guid assignId, decimal quantityUsed, decimal quantityRemaining)
            : base(id)
        {
            MaterialId = materialId;
            BatchId = batchId;
            AssignId = assignId;
            QuantityUsed = quantityUsed;
            QuantityRemaining = quantityRemaining;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        private MaterialUse() : base(Guid.NewGuid()) { }
    }
}
