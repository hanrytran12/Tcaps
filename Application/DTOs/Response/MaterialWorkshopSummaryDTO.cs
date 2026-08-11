namespace Application.DTOs.Response
{
    public class MaterialWorkshopSummaryDTO
    {
        public Guid Id { get; set; }
        public Guid WorkshopId { get; set; }
        public Guid AssignId { get; set; }
        public Guid AssignmentTransferRequestId { get; set; }
        public Guid SupplierId { get; set; }
        public int QuantitySend { get; set; }
        public int QuantityReceive { get; set; }
        public DateOnly ShipDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
