namespace Application.DTOs.Response
{
    public class MaterialSupplyDTO
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public Guid MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public string? BatchCode { get; set; }
        public Guid WorkshopId { get; set; }
        public string? WorkshopName { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string ReceiverName { get; set; }
        public int QuantitySend { get; set; }
        public int QuantityReceive { get; set; }
        public string Unit { get; set; }
        public DateTime DateShip { get; set; }
        public DateTime? DateReceive { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? LeadNote { get; set; }
    }
}
