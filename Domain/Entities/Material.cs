using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Material : AggregrateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string Unit { get; private set; }
        public string ImageURL { get; private set; } = string.Empty;

        public Material(Guid id, string name, string description, int quantity, decimal price, string unit, string imageURL)
            : base(id)
        {
            Name = name;
            Description = description;
            Quantity = quantity;
            Price = price;
            Unit = unit;
            ImageURL = imageURL;
        }

        private Material() : base(Guid.NewGuid()) { }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to increase must be non-negative.", nameof(amount));
            }
            Quantity += amount;

            AddDomainEvent(new MaterialStockUpdatedEvent(Name, Quantity, amount));
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to increase must be non-negative.", nameof(amount));
            }
            Quantity -= amount;
        }
    }
}
