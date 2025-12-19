using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Primitives;

namespace Domain.Entities
{
    public class FinalTransferRequest : Entity
    {
        public Guid AssignTransferRequestId { get; private set; }
        public decimal QuantityFinalSend { get; private set; }
        public decimal? QuantityFinalReceive { get; private set; }
        public string Status { get; private set; }
        public string? Note { get; private set; }
        public DateTime CreatedAt { get; private set; }
        
        public FinalTransferRequest(Guid id,  Guid assignTransferRequestId, decimal quantityFinalSend) : base(id)
        {
            AssignTransferRequestId = assignTransferRequestId;
            QuantityFinalSend = quantityFinalSend;
            Status = "Pending";
            CreatedAt = DateTime.Now;
        }
        
        public static FinalTransferRequest Create(Guid assignTransferRequestId, decimal quantityFinalSend)
        {
            return new FinalTransferRequest(Guid.NewGuid(), assignTransferRequestId, quantityFinalSend);
        }

        public void Approve(decimal quantityFinalReceive, string note)
        {
            Status = "Approved";
            QuantityFinalReceive = quantityFinalReceive;
            Note = note;
        }
    }
}
