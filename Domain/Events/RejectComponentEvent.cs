using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class RejectComponentEvent : INotification
    {
        public Guid ComponentId { get; set; }
        public Guid EvaluateId { get; set; }
        public int Quantity { get; set; }
        public int QuantityReject { get; set; }

        public RejectComponentEvent(Guid componentId, Guid evaluateId, int quantity, int quantityReject)
        {
            ComponentId = componentId;
            EvaluateId = evaluateId;
            Quantity = quantity;
            QuantityReject = quantityReject;
        }
    }
}
