namespace Application.DTOs.Response
{
    public class MaterialUsageSummaryDTO
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public decimal QuantityDivided { get; set; }
        public decimal QuantityStaffUsed { get; set; }
    }
}