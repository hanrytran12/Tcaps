using Domain.Primitives;

namespace Domain.Entities
{
    public class Inventory : Entity
    {
        public Guid MaterialId { get; private set; }
        public int Quantity { get; private set; }
        public DateOnly Date { get; private set; }
        public string ImageURL { get; private set; } = string.Empty;

        public Inventory(Guid Id, Guid materialId, int quantity, string imageURL)
            : base(Id)
        {
            MaterialId = materialId;
            Quantity = quantity;
            Date = DateOnly.FromDateTime(DateTime.Now);
            ImageURL = imageURL;
        }

        private Inventory() : base(Guid.NewGuid()) { }

        public static Inventory Create(Guid materialId, int quantity, string imageURL)
        {
            return new Inventory(Guid.NewGuid(), materialId, quantity, imageURL);
        }
    }
}
