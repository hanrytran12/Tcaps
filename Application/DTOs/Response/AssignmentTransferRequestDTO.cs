namespace Application.DTOs.Response
{
    public class AssignmentTransferRequestDTO
    {
        public Guid TransferRequestId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string WorkshopName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal CompletedQuantitySend { get; set; }
        public decimal CompletedQuantityReceive { get; set; }
        public string Note { get; set; } = string.Empty;
        public string NotLead { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
