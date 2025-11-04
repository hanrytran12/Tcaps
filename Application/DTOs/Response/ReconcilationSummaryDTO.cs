namespace Application.DTOs.Response
{
    public class ReconcilationSummaryDTO
    {
        public int QuantityTarget { get; set; }
        public decimal TotalSumbimttedQuantity { get; set; }
        public decimal TotalRejectedQuantity { get; set; }
        public decimal FinalCompletedQuantity { get; set; }

        public List<MaterialUsageSummaryDTO> MaterialUsageSummary { get; set; } = new List<MaterialUsageSummaryDTO>();
    }
}
