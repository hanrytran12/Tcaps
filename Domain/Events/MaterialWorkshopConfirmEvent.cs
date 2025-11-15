using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class MaterialWorkshopConfirmEvent : INotification
    {
        public Guid WorkshopId { get; set; }
        public int QuantitySend { get; set; }
        public int QuantityReceive { get; set; }
        public DateOnly ShipDate { get; set; }

        public MaterialWorkshopConfirmEvent(Guid workshopId, int quantitySend, int quantityReceive, DateOnly shipDate)
        {
            WorkshopId = workshopId;
            QuantitySend = quantitySend;
            QuantityReceive = quantityReceive;
            ShipDate = shipDate;
        }
    }
}
