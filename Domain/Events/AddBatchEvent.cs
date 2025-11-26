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
        public string BatchCode { get; set; }
        public decimal Quantity { get; set; }
        public AddBatchEvent(string batchCode, decimal quantity)
        {
            BatchCode = batchCode;
            Quantity = quantity;
        }
    }
}
