namespace Application.DTOs.Response
{
    public class ReworkRequestDTO
    {
        public Guid Id { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string QcName { get; set; } = string.Empty;
        public Guid? LeadId { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public Guid AssignmentId { get; set; }
        public decimal DefectiveQuantity { get; set; }
        public string NoteQc { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool RequiresMaterialDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? NextStepDeliveryDate { get; set; }
    }
}
