using Domain.Primitives;

namespace Domain.Entities
{
    public class Assignment : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateOnly ExpectedDeliveryDate { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public Assignment(Guid id, Guid batchId, Guid workshopId, int quantity, DateOnly startDate, DateOnly endDate, DateOnly expectedDeliveryDate, decimal unitPrice)
            : base(id)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            Status = "Pending";
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            ExpectedDeliveryDate = expectedDeliveryDate;
            UnitPrice = unitPrice;
        }

        private Assignment() : base(Guid.NewGuid()) { }

        public static Assignment Create(Guid batchId, Guid workshopId, int quantity, DateOnly startDate, DateOnly endDate, DateOnly expectedDeliveryDate, decimal unitPrice)
        {
            return new Assignment(Guid.NewGuid(), batchId, workshopId, quantity, startDate, endDate, expectedDeliveryDate, unitPrice);
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void UpdateWhenQcConfrimed()
        {
            if (StepOrder == 1)
            {
                UpdateStatus("InProgress");
            }

            else if (StepOrder > 1)
            {
                UpdateStatus("Ready");
            }
        }
    }
}
