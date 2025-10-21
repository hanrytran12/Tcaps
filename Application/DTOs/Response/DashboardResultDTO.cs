namespace Application.DTOs.Response
{
    public class DashboardResultDTO
    {
        public DashboardStatsDTO Stats { get; set; }
        public IEnumerable<DashboardBatchDetailDTO> Batches { get; set; }
    }
}
