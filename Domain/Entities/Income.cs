using Domain.Primitives;

namespace Domain.Entities
{
    public class Income : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid ProductionId { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Income(Guid id, Guid batchId, Guid productionId, Guid userId, int quantity, decimal totalPrice)
            : base(id)
        {
            BatchId = batchId;
            ProductionId = productionId;
            UserId = userId;
            Quantity = quantity;
            TotalPrice = totalPrice;
            CreatedAt = DateTime.Now;
        }

        private Income() : base(Guid.NewGuid()) { }

        public static Income Create(Guid batchId, Guid productionId, Guid userId, int quantity, decimal totalPrice)
        {
            return new Income(Guid.NewGuid(), batchId, productionId, userId, quantity, totalPrice);
        }

        public void ReduceQuantity(int quantityError)
        {
            if (quantityError <= 0)
                return;

            if (Quantity < quantityError)
                throw new InvalidOperationException("Không thể giảm số lượng vượt quá số lượng hiện tại.");

            Quantity -= quantityError;
        }
    }
}
