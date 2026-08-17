using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Material : AggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public string Unit { get; private set; }

        public Material(Guid id, string name, string description, string unit)
            : base(id)
        {
            Name = name;
            Description = description;
            Quantity = 0;
            Price = 0;
            Unit = unit;
        }

        private Material() : base(Guid.NewGuid()) { }

        public static Material Create(string name, string description, string unit)
        {
            return new Material(Guid.NewGuid(), name, description, unit);
        }

        public void IncreasePrice(decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Price to increase must be non-negative.", nameof(price));
            }
            Price += price;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to increase must be non-negative.", nameof(amount));
            }
            Quantity += amount;

            AddDomainEvent(new MaterialStockUpdatedEvent(Guid.Empty, Name, Quantity, amount));
        }

        public void DecreaseQuantity(int amount, Guid userId)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to increase must be non-negative.", nameof(amount));
            }
            Quantity -= amount;

            AddDomainEvent(new MaterialStockUpdatedEvent(userId, Name, Quantity, amount * -1));
        }
    }
}
