using MediatR;

namespace Domain.Events
{
    public class TransferRequestInProgressEvent : INotification
    {
        public Guid AssignmentId { get; set; }
        public Guid AssignmentTransferRequestId { get; set; }
        public Guid? ReworkRequestId { get; set; }
        public decimal QuantitySend { get; set; }
        public decimal QuantityReject { get; set; }
        public Guid SupplierId { get; set; }

        public TransferRequestInProgressEvent(Guid assignmentId, Guid assignmentTransferRequestId, Guid? reworkRequestId, decimal quantitySend, decimal quantityReject, Guid supplierId)
        {
            AssignmentId = assignmentId;
            AssignmentTransferRequestId = assignmentTransferRequestId;
            ReworkRequestId = reworkRequestId;
            QuantitySend = quantitySend;
            QuantityReject = quantityReject;
            SupplierId = supplierId;
        }
    }
}
