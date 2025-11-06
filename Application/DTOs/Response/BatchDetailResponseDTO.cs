namespace Application.DTOs.Response
{
    public class BatchDetailResponseDTO
    {
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly EndDate { get; set; }
        public int DurationDay { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public double ProgressPercentage { get; set; }
        public IEnumerable<DashboardAssignmentDTO> Assignments { get; set; } = new List<DashboardAssignmentDTO>();
    }
}
