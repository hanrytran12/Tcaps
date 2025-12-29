using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ProductionReportedEvent : INotification
    {
        public Guid AssignId { get; set; }
        public Guid StaffId { get; set; }
        public decimal Quantity { get; set; }

        public ProductionReportedEvent(Guid assignId, Guid staffId, decimal quantity)
        {
            AssignId = assignId;
            StaffId = staffId;
            Quantity = quantity;
        }
    }
}
