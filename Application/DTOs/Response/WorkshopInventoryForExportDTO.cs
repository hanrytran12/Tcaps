namespace Application.DTOs.Response
{
    public class WorkshopInventoryForExportDTO
    {
        public string MaterialName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
