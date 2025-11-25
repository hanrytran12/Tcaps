namespace Application.DTOs.Response
{
    public class ReworkRequestDTO
    {
        public string BatchCode { get; set; } = string.Empty;
        public string QcName { get; set; } = string.Empty;
        public string WorkshopName { get; set; } = string.Empty;
        public decimal DefectiveQuantity { get; set; }
        public string NoteQc { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? NextStepDeliveryDate { get; set; }
    }
}
