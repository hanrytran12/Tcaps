using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class CompletedMaterialSupplyEvent : INotification
    {
        public Guid SupplyId { get; set; }
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
        public CompletedMaterialSupplyEvent(Guid supplyId, Guid materialId, int quantity)
        {
            SupplyId = supplyId;
            MaterialId = materialId;
            Quantity = quantity;
        }
    }
}
