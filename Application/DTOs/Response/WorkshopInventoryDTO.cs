namespace Application.DTOs.Response
{
    public class WorkshopInventoryDTO
    {
        public Guid Id { get; set; }
        public Guid WorkshopId { get; set; }
        public Guid MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal HoldingQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
    }
}
