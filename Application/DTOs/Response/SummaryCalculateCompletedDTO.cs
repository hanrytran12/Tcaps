namespace Application.DTOs.Response
{
    public class SummaryCalculateCompletedDTO
    {
        public decimal TotalSubmitted { get; set; }
        public decimal TotalRejected { get; set; }
        public decimal TotalCompleted { get; set; }
    }
}
