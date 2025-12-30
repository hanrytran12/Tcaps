using System.Text.Json.Serialization;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Assignment : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public int? StepOrder { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateOnly? ExpectedDeliveryDate { get; private set; }
        public DateOnly? DateCompleted { get; private set; }
        public bool RequiresMaterialDelivery { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        [JsonIgnore]
        public Batch Batch { get; private set; } = null!;
        public Assignment(Guid id, Guid batchId, Guid workshopId, int? stepOrder, int quantity, DateOnly startDate, DateOnly endDate, DateOnly? expectedDeliveryDate, decimal unitPrice, bool requiresMaterialDelivery)
            : base(id)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            StepOrder = stepOrder;
            Status = "Planned";
            CreatedAt = DateTime.Now;
            ExpectedDeliveryDate = expectedDeliveryDate;
            UnitPrice = unitPrice;
            RequiresMaterialDelivery = requiresMaterialDelivery;
        }

        private Assignment() : base(Guid.NewGuid()) { }

        public static Assignment Create(Guid batchId, Guid workshopId, int? stepOrder, int quantity, DateOnly startDate, DateOnly endDate, DateOnly? expectedDeliveryDate, decimal unitPrice, bool requiresMaterialDelivery)
        {
            return new Assignment(Guid.NewGuid(), batchId, workshopId, stepOrder, quantity, startDate, endDate, expectedDeliveryDate, unitPrice, requiresMaterialDelivery);
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void UpdateWhenQcConfirmed(bool isFirstStepInPlan)
        {
            if (Status != "Planned")
            {
                return;
            }

            if (isFirstStepInPlan)
            {
                UpdateStatus("InProgress");
            }
            else
            {
                UpdateStatus("Ready");
            }
        }

        public void Active()
        {
            UpdateStatus("InProgress");
        }

        public void UpdateDateComplete()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateCompleted = today;
        }
    }
}
