namespace Application.DTOs.Response
{
    public class DashboardAssignmentDTO
    {
        public Guid AssignmentId { get; set; }
        public string WorkshopName { get; set; }
        public int Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal UnitPrice { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
