namespace Application.DTOs.Response
{
    public class BatchAssignmentResponseDTO
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public int? StepOrder { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
        public DateOnly? DateCompleted { get; set; }
        public bool RequiresMaterialDelivery { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
