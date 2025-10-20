namespace Application.DTOs.Response
{
    public class DashboardStatsDTO
    {
        public int TotalBatches { get; set; }
        public decimal InProgressBatches { get; set; }
        public int CompletedBatches { get; set; }
    }
}
