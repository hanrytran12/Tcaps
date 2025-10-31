namespace Application.DTOs.Response
{
    public class BatchDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
