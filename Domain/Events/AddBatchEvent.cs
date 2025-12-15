using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class AddBatchEvent : INotification
    {
        public Guid UserId { get; set; }
        public string BatchCode { get; set; }
        public decimal Quantity { get; set; }
        public AddBatchEvent(Guid userId, string batchCode, decimal quantity)
        {
            UserId = userId;
            BatchCode = batchCode;
            Quantity = quantity;
        }
    }
}
