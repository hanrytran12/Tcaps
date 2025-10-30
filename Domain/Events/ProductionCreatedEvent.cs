using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ProductionCreatedEvent : INotification
    {
        public Guid AssignId { get; }
        public Guid StaffId { get; }
        public int Quantity { get; }

        public ProductionCreatedEvent(Guid assignId, Guid staffId, int quantity)
        {
            AssignId = assignId;
            StaffId = staffId;
            Quantity = quantity;
        }
    }
}
