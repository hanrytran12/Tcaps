using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialWorkshop : Entity
    {
        public Guid WorkshopId { get; private set; }
        public Guid AssignId { get; private set; }
        public Guid AssignmentTransferRequestId { get; private set; }
        public Guid SupplierId { get; private set; }
        public int QuantitySend { get; private set; }
        public int QuantityReceive { get; private set; }
        public DateOnly ShipDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public MaterialWorkshop(Guid id, Guid workshopId, Guid assignId, Guid assignTransferRequestId, Guid supplierId, int quantitySend) : base(id)
        {
            WorkshopId = workshopId;
            AssignId = assignId;
            AssignmentTransferRequestId = assignTransferRequestId;
            SupplierId = supplierId;
            QuantitySend = quantitySend;
            CreatedAt = DateTime.Now;
            Status = "Pending";
        }

        private MaterialWorkshop() : base(Guid.Empty) { }

        public static MaterialWorkshop Create(Guid workshopId, Guid assignId, Guid assignTransferRequestId, Guid supplierId, int quantitySend)
        {
            return new MaterialWorkshop(Guid.NewGuid(), workshopId, assignId, assignTransferRequestId, supplierId, quantitySend);
        }

        public void Confirmed() => Status = "Confirmed";
        public void Update(int quantityReceive)
        {
            ShipDate = DateOnly.FromDateTime(DateTime.UtcNow);
            QuantityReceive = quantityReceive;
        }
    }
}
