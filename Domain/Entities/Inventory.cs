using Domain.Primitives;

namespace Domain.Entities
{
    public class Inventory : Entity
    {
        public Guid MaterialId { get; private set; }
        public int Quantity { get; private set; }
        public DateOnly Date { get; private set; }

        public Inventory(Guid Id, Guid materialId, int quantity)
            : base(Id)
        {
            MaterialId = materialId;
            Quantity = quantity;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        private Inventory() : base(Guid.NewGuid()) { }
    }
}
