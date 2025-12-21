using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ApproveFinalTransferRequestEvent : INotification
    {
        public Guid BatchId { get; set; }
        public decimal QuantityComplete { get; set; }
        public decimal QuantityError { get; set; }

        public ApproveFinalTransferRequestEvent(Guid batchId, decimal quantityComplete, decimal quantityError)
        {
            BatchId = batchId;
            QuantityComplete = quantityComplete;
            QuantityError = quantityError;
        }
    }
}
