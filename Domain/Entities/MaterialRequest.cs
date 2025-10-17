using Domain.Primitives;

namespace Domain.Entities
{
    public class MaterialRequest : Entity
    {
        public Guid MaterialId { get; private set; }
        public Guid UserId { get; private set; }
        public Guid BatchId { get; private set; }
        public decimal QuantityRequest { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public DateOnly Date { get; private set; }

        public MaterialRequest(Guid id, Guid materialId, Guid userId, Guid batchId, decimal quantityRequest, string status, string note)
            : base(id)
        {
            MaterialId = materialId;
            UserId = userId;
            BatchId = batchId;
            QuantityRequest = quantityRequest;
            Status = status;
            Note = note;
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        private MaterialRequest() : base(Guid.NewGuid()) { }
    }
}
