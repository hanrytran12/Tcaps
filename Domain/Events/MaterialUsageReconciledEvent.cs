using MediatR;

namespace Domain.Events
{
    public class MaterialUsageReconciledEvent : INotification
    {
        public string BatchCode { get; set; } = string.Empty;
        public Guid MaterialUseId { get; set; }
        public Guid UserId { get; set; }
        public decimal ReconciledQuantity { get; set; }

        public MaterialUsageReconciledEvent(string batchCode, Guid materialUseId, Guid userId, decimal reconciledQuantity)
        {
            BatchCode = batchCode;
            MaterialUseId = materialUseId;
            UserId = userId;
            ReconciledQuantity = reconciledQuantity;
        }
    }
}
