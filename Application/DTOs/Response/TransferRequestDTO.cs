namespace Application.DTOs.Response
{
    public class TransferRequestDTO
    {
        public Guid AssignmentId { get; set; }
        public Guid TransferRequestId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public CreatedBy CreatedBy { get; set; } = new CreatedBy();
        public List<ReconciliationMaterials> ReconciliationMaterials { get; set; } = new List<ReconciliationMaterials>();
    }

    public class CreatedBy
    {
        public Guid QcId { get; set; }
        public string QcName { get; set; } = string.Empty;
    }

    public class ReconciliationMaterials
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public decimal ReconciliationQuantity { get; set; }
    }
}
