using Domain.Primitives;

namespace Domain.Entities
{
    public class WorkshopInventory : AggregrateRoot
    {
        public Guid WorkshopId { get; private set; }
        public Guid MaterialId { get; private set; }
        public decimal Quantity { get; private set; }

        public WorkshopInventory(Guid Id, Guid workshopId, Guid materialId, decimal quantity) : base(Id)
        {
            WorkshopId = workshopId;
            MaterialId = materialId;
            Quantity = quantity;
        }

        private WorkshopInventory() : base(Guid.NewGuid()) { }
    }
}
