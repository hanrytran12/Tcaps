namespace Application.DTOs.Response
{
    public class BatchForQCDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string BatchCode { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public AssignmentDTO Assignment { get; set; } = new AssignmentDTO();
    }
}
