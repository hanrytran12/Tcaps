namespace Application.DTOs.Response
{
    public class ReworkRequestResponseDTO
    {
        public Guid Id { get; set; }
        public Guid QcId { get; set; }
        public Guid AssignmentId { get; set; }
        public decimal DefectiveQuantity { get; set; }
        public string NoteQc { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? NextStepDeliveryDate { get; set; }
    }
}
