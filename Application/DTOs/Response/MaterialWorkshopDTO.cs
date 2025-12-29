namespace Application.DTOs.Response
{
    public class MaterialWorkshopDTO
    {
        public Guid Id { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public string BatchCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Guid AssignId { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int QuantitySend { get; set; }
        public int QuantityReceive { get; set; }
        public DateOnly ShipDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
