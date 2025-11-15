using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialWorkshop : Entity
    {
        public Guid WorkshopId { get; private set; }
        public Guid AssignId { get; private set; }
        public int QuantitySend { get; private set; }
        public int QuantityReceive { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Unit { get; private set; } = string.Empty;
        public DateOnly ShipDate { get; private set; }
        public string Image { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public MaterialWorkshop(Guid id, Guid workshopId, Guid assignId, int quantitySend) : base(id)
        {
            WorkshopId = workshopId;
            AssignId = assignId;
            QuantitySend = quantitySend;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = "Pending";
        }

        public static MaterialWorkshop Create(Guid workshopId, Guid assignId, int quantitySend)
        {
            return new MaterialWorkshop(Guid.NewGuid(), workshopId, assignId, quantitySend);
        }

        public void Confirmed() => Status = "Confirmed";
        public void Update(int quantityReceive)
        {
            ShipDate = DateOnly.FromDateTime(DateTime.UtcNow);
            QuantityReceive = quantityReceive;
        }
    }
}
