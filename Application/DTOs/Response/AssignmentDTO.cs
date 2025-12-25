namespace Application.DTOs.Response
{
    public class AssignmentDTO
    {
        public Guid AssignmentId { get; set; }
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; }
    }
}
