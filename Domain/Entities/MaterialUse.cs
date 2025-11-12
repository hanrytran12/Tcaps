using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialUse : Entity
    {
        public Guid MaterialId { get; private set; }
        public Guid BatchId { get; private set; }
        public Guid AssignId { get; private set; }
        public Guid? ReworkRequestId { get; private set; }
        public decimal QuantityDivide { get; private set; }
        public decimal QuantityStaffUse { get; private set; }
        public decimal ReconciledQuantity { get; private set; }
        public decimal QuantityRequest { get; private set; }
        public DateOnly Date { get; private set; }

        public MaterialUse(Guid id, Guid materialId, Guid batchId, Guid assignId, decimal quantityDivide, Guid? reworkRequestId)
            : base(id)
        {
            MaterialId = materialId;
            BatchId = batchId;
            AssignId = assignId;
            QuantityDivide = quantityDivide;
            Date = DateOnly.FromDateTime(DateTime.Now);
            ReworkRequestId = reworkRequestId;
        }

        private MaterialUse() : base(Guid.NewGuid()) { }

        public static MaterialUse Create(Guid materialId, Guid batchId, Guid assignId, decimal quantityDivide, Guid? reworkRequestId)
        {
            return new MaterialUse(Guid.NewGuid(), materialId, batchId, assignId, quantityDivide, reworkRequestId);
        }

        public void IncreaseQuantityStaffUse(int quantityProduction)
        {
            if (quantityProduction < 0)
                throw new InvalidOperationException("Quantity cannot be negative.");

            QuantityStaffUse += quantityProduction;
        }

        public void UpdateReconciledQuantity(decimal quantity)
        {
            if (quantity < 0)
                throw new InvalidOperationException("Quantity cannot be negative.");

            ReconciledQuantity += quantity;
        }

        public void IncreaseQuantityRequest(int quantityProduction)
        {
            if (quantityProduction < 0)
                throw new InvalidOperationException("Quantity cannot be negative.");

            QuantityRequest += quantityProduction;
        }
    }
}
