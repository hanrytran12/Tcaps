using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class AddBatchForAdminEvent : INotification
    {
        public string BatchCode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }

        public AddBatchForAdminEvent(string batchCode, decimal quantity)
        {
            BatchCode = batchCode;
            Quantity = quantity;
        }
    }
}
