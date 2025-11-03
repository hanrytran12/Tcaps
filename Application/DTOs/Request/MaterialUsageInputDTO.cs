namespace Application.DTOs.Request
{
    public class MaterialUsageInputDTO
    {
        public Guid MaterialId { get; set; }
        public int QuantityUsed { get; set; }
    }
}
