namespace Application.DTOs.Response
{
    public class WorkshopInventoryForExportDTO
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal HoldingQuantity { get; set; }
    }
}
