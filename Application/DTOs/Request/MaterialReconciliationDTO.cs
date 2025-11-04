namespace Application.DTOs.Request
{
    public class MaterialReconciliationDTO
    {
        public Guid MaterialId { get; set; }
        public decimal ReconciliationQuantity { get; set; }
    }
}
