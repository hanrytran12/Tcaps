namespace Application.DTOs.Response
{
    public class BatchDetailResponseDTO
    {
        public Guid UserId { get; set; }
        public string LeadName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly EndDate { get; set; }
        public int DurationDay { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal LostQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public double ProgressPercentage { get; set; }
        public IEnumerable<DashboardAssignmentDTO> Assignments { get; set; } = new List<DashboardAssignmentDTO>();
    }
}
