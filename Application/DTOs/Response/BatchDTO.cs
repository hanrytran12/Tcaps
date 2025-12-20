namespace Application.DTOs.Response
{
    public class BatchDTO
    {
        public Guid BatchId { get; set; }
        public Guid UserId { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
