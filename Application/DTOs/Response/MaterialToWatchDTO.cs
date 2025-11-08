namespace Application.DTOs.Response
{
    public class MaterialToWatchDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
