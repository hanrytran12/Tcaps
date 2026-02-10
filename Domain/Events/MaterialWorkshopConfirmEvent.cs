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
        public Guid? UserId { get; set; }

        public MaterialWorkshopConfirmEvent(Guid workshopId, int quantitySend, int quantityReceive, Guid? userId)
        {
            WorkshopId = workshopId;
            QuantitySend = quantitySend;
            QuantityReceive = quantityReceive;
            UserId = userId;
        }
    }
}
