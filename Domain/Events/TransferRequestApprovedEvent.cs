using MediatR;

namespace Domain.Events
{
    public class TransferRequestApprovedEvent : INotification
    {
        public Guid AssignmentId { get; set; }
        public Guid? ReworkRequestId { get; set; }
        public decimal QuantitySend { get; set; }


        public TransferRequestApprovedEvent(Guid assignmentId, Guid? reworkRequestId, decimal quantitySend)
        {
            AssignmentId = assignmentId;
            ReworkRequestId = reworkRequestId;
            QuantitySend = quantitySend;
        }
    }
}
