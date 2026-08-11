namespace Application.DTOs.Response
{
    public class IncomeHistoryDTO
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public Guid ProductionId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
