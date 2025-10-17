using Domain.Primitives;

namespace Domain.Entities
{
    public class Product : Entity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public Product(Guid id, string code, string name, string image, string description)
            : base(id)
        {
            Code = code;
            Name = name;
            Image = image;
            Description = description;
        }

        private Product() : base(Guid.NewGuid()) { }
    }
}
