namespace Application.DTOs.Response
{
    public class InventoryResponseDTO
    {
        public Guid Id { get; set; }
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }
}
