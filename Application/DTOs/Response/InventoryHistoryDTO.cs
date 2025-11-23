namespace Application.DTOs.Response
{
    public class InventoryHistoryDTO
    {
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

        public List<InventoryDTO> Inventories { get; set; } = new();
    }
}
