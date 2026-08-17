using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Inventory : AggregateRoot
    {
        public Guid MaterialId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Price { get; private set; }
        public string ImageURL { get; private set; } = string.Empty;

        public Inventory(Guid Id, Guid materialId, int quantity, string imageURL, decimal price)
            : base(Id)
        {
            MaterialId = materialId;
            Quantity = quantity;
            Date = DateTime.Now;
            ImageURL = imageURL;
            Price = price;
        }

        private Inventory() : base(Guid.NewGuid()) { }

        public static Inventory Create(Guid materialId, int quantity, string imageURL, decimal price)
        {
            var inventory = new Inventory(Guid.NewGuid(), materialId, quantity, imageURL, price);
            inventory.AddDomainEvent(new InventoryAddEvent(inventory.MaterialId, inventory.Quantity, inventory.Price));
            return inventory;
        }
    }
}
