using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ComponentResolvedEvent : INotification
    {
        public Guid Id { get; set; }
        public Guid EvaluateId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public ComponentResolvedEvent(Guid id, Guid evaluateId, int quantity, string status)
        {
            Id = id;
            EvaluateId = evaluateId;
            Quantity = quantity;
            Status = status;
        }
    }
}
