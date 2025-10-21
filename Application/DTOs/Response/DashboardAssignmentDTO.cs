namespace Application.DTOs.Response
{
    public class DashboardAssignmentDTO
    {
        public Guid WorkshopId { get; set; }
        public int Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
