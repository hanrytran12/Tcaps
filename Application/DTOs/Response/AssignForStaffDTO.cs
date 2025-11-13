namespace Application.DTOs.Response
{
    public class AssignForStaffDTO
    {
        public Guid AssignId { get; set; }
        public string? BatchesCode { get; set; }
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public int StepOrder { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
    }
}
