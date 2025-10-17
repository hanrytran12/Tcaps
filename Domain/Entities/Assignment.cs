using Domain.Primitives;

namespace Domain.Entities
{
    public class Assignment : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public Assignment(Guid id, Guid batchId, Guid workshopId, int quantity, DateTime startDate, DateTime endDate)
            : base(id)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            Status = "Pending";
            CreatedAt = new DateOnly();
        }

        private Assignment() : base(Guid.NewGuid()) { }
    }
}
