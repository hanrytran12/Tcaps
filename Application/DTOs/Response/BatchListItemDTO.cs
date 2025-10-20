namespace Application.DTOs.Response
{
    public class BatchListItemDTO
    {
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double ProgressPercentage { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
