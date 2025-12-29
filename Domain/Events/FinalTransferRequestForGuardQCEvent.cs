using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class FinalTransferRequestForGuardQCEvent : INotification
    {
        public decimal QuantitySend { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string WorkshopName { get; set; } = string.Empty;

        public FinalTransferRequestForGuardQCEvent(decimal quantitySend, string batchCode, string workshopName)
        {
            QuantitySend = quantitySend;
            BatchCode = batchCode;
            WorkshopName = workshopName;
        }
    }
}
