namespace Application.Features.Users.Queries.GetStaffPerformance
{
    public class StaffPerformanceDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal TotalIncome { get; set; }
        public int TotalQuantitySold { get; set; }
    }
}
