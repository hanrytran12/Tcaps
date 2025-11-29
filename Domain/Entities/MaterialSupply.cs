using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialSupply : Entity
    {
        public Guid RequestId { get; private set; }
        public Guid MaterialId { get; private set; }
        public Guid SupplierId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public int QuantitySend { get; private set; }
        public int? QuantityReceive { get; private set; }
        public string Unit { get; private set; }
        public DateOnly DateShip {  get; private set; }
        public DateOnly? DateReceive { get; private set; }
        public string Status { get; private set; } = "Pending";

        public MaterialSupply(Guid id, Guid requestId, Guid materialId, Guid supplierId, Guid workshopId, int quantitySend, string unit, DateOnly dateShip, string? status) : base(id)
        {
            RequestId = requestId;
            MaterialId = materialId;
            SupplierId = supplierId;
            WorkshopId = workshopId;
            QuantitySend = quantitySend;
            Unit = unit;
            DateShip = dateShip;
            Status = status;
        }

        public static MaterialSupply Create(Guid requestId, Guid materialId, Guid supplierId, Guid workshopId, int quantitySend, string unit, DateOnly dateShip, Guid leadId)
        {
            var status = supplierId == leadId ? "InProgress" : "Pending";
            return new MaterialSupply(Guid.NewGuid(), requestId, materialId, supplierId, workshopId, quantitySend, unit, dateShip, status);
        }

        public void MarkAsInProgress() => Status = "InProgress";
        public void MarkAsCompleted(int quantityReceive)
        {
            QuantityReceive = quantityReceive;
            Status = "Completed";
            DateReceive = DateOnly.FromDateTime(DateTime.Now);
        }

        public void MarkAsApprovedByAdmin() => Status = "Approved";
    }
}
