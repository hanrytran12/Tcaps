using Domain.Primitives;

namespace Domain.Entities
{
    public class WorkshopInventory : AggregateRoot
    {
        public Guid WorkshopId { get; private set; }
        public Guid MaterialId { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal HoldingQuantity { get; private set; }
        public decimal AvailableQuantity => Quantity - HoldingQuantity;

        public WorkshopInventory(Guid Id, Guid workshopId, Guid materialId, decimal quantity) : base(Id)
        {
            WorkshopId = workshopId;
            MaterialId = materialId;
            Quantity = quantity;
        }

        private WorkshopInventory() : base(Guid.NewGuid()) { }

        public static WorkshopInventory Create(Guid workshopId, Guid materialId, decimal quantity)
        {
            return new WorkshopInventory(Guid.NewGuid(), workshopId, materialId, quantity);
        }

        public void HoldStock()
        {
            if (AvailableQuantity > 0)
            {
                HoldingQuantity += Quantity;
            }
        }

        public void IncreaseQuantity(decimal quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            Quantity += quantity;
        }

        public void DecreaseQuantity(decimal quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            Quantity -= quantity;
            HoldingQuantity = 0;
        }
    }
}
