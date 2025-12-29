using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class AddMaterialSupplyForAdminEvent : INotification
    {
        public Guid MaterialId { get; set; }
        public decimal QuantitySend { get; set; }
        public DateOnly DateShip { get; set; }

        public AddMaterialSupplyForAdminEvent(Guid materialId, decimal quantitySend, DateOnly dateShip)
        {
            MaterialId = materialId;
            QuantitySend = quantitySend;
            DateShip = dateShip;
        }
    }
}
