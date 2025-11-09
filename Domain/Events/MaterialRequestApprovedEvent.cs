using MediatR;

namespace Domain.Events
{
    public class MaterialRequestApprovedEvent : INotification
    {
        public Guid MaterialId { get; }
        public Guid Qc_Id { get; }
        public Guid BatchId { get; }
        public decimal QuantityRequest { get; }

        public MaterialRequestApprovedEvent(Guid materialId, Guid qcId, Guid batchId, decimal quantityRequest)
        {
            MaterialId = materialId;
            Qc_Id = qcId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
        }

    }
}
