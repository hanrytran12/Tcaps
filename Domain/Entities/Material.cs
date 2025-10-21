using Domain.Primitives;

namespace Domain.Entities
{
    public class Material : Entity
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
    }
}
