using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialWorkshop : Entity
    {
        public Guid WorkshopId { get; private set; }
        public int QuantitySend { get; private set; }
        public int QuantityReceive { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public DateOnly ShipDate { get; private set; }
        public string Image { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; }

        public MaterialWorkshop(Guid id, Guid workshopId, int quantitySend, int quantityReceive, string name, string unit, string image, string status) : base(id)
        {
            WorkshopId = workshopId;
            QuantitySend = quantitySend;
            QuantityReceive = quantityReceive;
            Name = name;
            Unit = unit;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Image = image;
            Status = status;
        }

        public static MaterialWorkshop Create(Guid workshopId, int quantitySend, int quantityReceive, string name, string unit, string image, string status)
        {
            return new MaterialWorkshop(Guid.NewGuid(), workshopId, quantitySend, quantityReceive, name, unit, image, status);
        }

        public void Confirmed() => Status = "Confirmed";
        public void Update(int quantityReceive)
        {
            ShipDate = DateOnly.FromDateTime(DateTime.UtcNow);
            QuantityReceive = quantityReceive;
        }
    }
}
