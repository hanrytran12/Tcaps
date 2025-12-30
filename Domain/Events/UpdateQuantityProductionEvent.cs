using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class UpdateQuantityProductionEvent : INotification
    {
        public Guid UserId { get; set; }
        public decimal QuantitySend { get; set; }
        public decimal QuantityReceive { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string BatchCode { get; set; } = string.Empty;

        public UpdateQuantityProductionEvent(Guid userId, decimal quantitySend, decimal quantityReceive, DateOnly date, TimeOnly time, string batchCode)
        {
            UserId = userId;
            QuantitySend = quantitySend;
            QuantityReceive = quantityReceive;
            Date = date;
            Time = time;
            BatchCode = batchCode;
        }
    }
}
