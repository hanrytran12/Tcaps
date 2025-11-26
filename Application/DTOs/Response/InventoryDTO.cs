namespace Application.DTOs.Response
{
    public class InventoryDTO
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
