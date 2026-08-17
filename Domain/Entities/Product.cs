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
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
                public ICollection<Batch> Batches { get; private set; } = new List<Batch>();

        public Product(Guid id, string code, string name, string image, string description)
            : base(id)
        {
            Code = code;
            Name = name;
            Image = image;
            Description = description;
            CreatedAt = DateTime.Now;
        }

        private Product() : base(Guid.NewGuid()) { }

        public static Product Create(string code, string name, string image, string description)
        {
            return new Product(Guid.NewGuid(), code, name, image, description);
        }

        public void UpdateDetails(string name, string description, string imageURL)
        {
            this.Name = name;
            this.Description = description;
            this.Image = imageURL;
        }

        public void MarkAsDeleted()
        {
            this.IsDeleted = true;
        }
    }
}
