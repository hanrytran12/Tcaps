using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class CreateMaterialWorkshopEvent : INotification
    {
        public Guid WorkshopPreviousId { get; set; }
        public Guid WorkshopAfterId { get; set; }
        public decimal QuantitySend { get; set; }
        public string BatchCode { get; set; }

        public CreateMaterialWorkshopEvent(Guid workshopPreviousId, Guid workshopAfterId, decimal quantitySend, string batchCode)
        {
            WorkshopPreviousId = workshopPreviousId;
            WorkshopAfterId = workshopAfterId;
            QuantitySend = quantitySend;
            BatchCode = batchCode;
        }
    }
}
