using Domain.Primitives;

namespace Domain.Entities
{
    public class Income : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid ProductionId { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateOnly CreatedAt { get; private set; }

        public Income(Guid id, Guid batchId, Guid productionId, Guid userId, int quantity, decimal unitPrice, decimal totalPrice)
            : base(id)
        {
            BatchId = batchId;
            ProductionId = productionId;
            UserId = userId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            CreatedAt = new DateOnly();
        }

        private Income() : base(Guid.NewGuid()) { }
    }
}
