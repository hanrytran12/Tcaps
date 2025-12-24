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
        public decimal Quantity { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string BatchCode { get; set; } = string.Empty;

        public UpdateQuantityProductionEvent(Guid userId, decimal quantity, DateOnly date, TimeOnly time, string batchCode)
        {
            UserId = userId;
            Quantity = quantity;
            Date = date;
            Time = time;
            BatchCode = batchCode;
        }
    }
}
