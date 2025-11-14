using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class EvaluateCreatedEvent : INotification
    {
        public Guid EvaluateId { get; }
        public Guid ProductionId { get; }
        public Guid? UserId { get; }
        public int QuantityError { get; }
        public int QuantitySuccess { get; }
        public string Note { get; } = string.Empty;
        public string Status { get; }

        public EvaluateCreatedEvent(Guid evaluateId, Guid productionId, Guid userId, int quantityError, int quantitySucess, string note, string status)
        {
            EvaluateId = evaluateId;
            ProductionId = productionId;
            UserId = userId;
            QuantityError = quantityError;
            QuantitySuccess = quantitySucess;
            Note = note;
            Status = status;
        }
    }
}
