using Domain.Primitives;

namespace Domain.Entities
{
    public class Product : Entity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }

        public Product(Guid id, string code, string name, string image, string description)
            : base(id)
        {
            Code = code;
            Name = name;
            Image = image;
            Description = description;
        }

        private Product() : base(Guid.NewGuid()) { }

        public static Product Create(string code, string name, string image, string description)
        {
            return new Product(Guid.NewGuid(), code, name, image, description);
        }

        public void UpdateDetails(string? name, string? description)
        {
            this.Name = !string.IsNullOrWhiteSpace(name) ? name : this.Name;
            this.Description = !string.IsNullOrWhiteSpace(description) ? description : this.Description;
        }

        public void MarkAsDeleted()
        {
            this.IsDeleted = true;
        }
    }
}
