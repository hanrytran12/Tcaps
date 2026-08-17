namespace Application.DTOs.Response
{
    public class BatchResponseDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal LostQuantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
        public List<BatchAssignmentResponseDTO> Assignments { get; set; } = new();
    }
}
