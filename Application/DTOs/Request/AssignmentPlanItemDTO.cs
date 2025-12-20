namespace Application.DTOs.Request
{
    public class AssignmentPlanItemDTO
    {
        public Guid WorkshopId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
        public bool RequiresMaterialDelivery { get; set; }
    }
}
