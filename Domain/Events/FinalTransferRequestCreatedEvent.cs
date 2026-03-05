using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class FinalTransferRequestCreatedEvent : INotification
    {
        public Guid Id { get; }
        public Guid AssignTransferRequestId { get; }
        public decimal QuantityFinalSend { get; }
        public string? Note { get; }

        public FinalTransferRequestCreatedEvent(Guid id, Guid assignTransferRequestId, decimal quantityFinalSend, string? note)
        {
            Id = id;
            AssignTransferRequestId = assignTransferRequestId;
            QuantityFinalSend = quantityFinalSend;
            Note = note;
        }
    }
}
