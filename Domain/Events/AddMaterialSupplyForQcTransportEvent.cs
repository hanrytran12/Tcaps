using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class AddMaterialSupplyForQcTransportEvent : INotification
    {
        public Guid QcId { get; set; }
        public Guid RequestId { get; set; }
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
        public AddMaterialSupplyForQcTransportEvent(Guid qcId, Guid requestId, Guid materialId, int quantity)
        {
            QcId = qcId;
            RequestId = requestId;
            MaterialId = materialId;
            Quantity = quantity;
        }
    }
}
