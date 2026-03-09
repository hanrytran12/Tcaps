namespace Application.DTOs.Response
{
    public class TaskTransferRequestDTO
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public string BatchCode { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public Guid QcTransportId { get; set; }
        public string QcTransportName { get; set; }
        public Guid? MaterialRequestId { get; set; }
        public Guid? AssignmentTransferId { get; set; }
        public string MaterialName { get; set; }
        public int QuantityRequest { get; set; }
        public int CompleteQuantity { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DateToGo { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public string NextWorkshopName { get; set; }
    }
}
