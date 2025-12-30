using Domain.Primitives;

namespace Domain.Entities
{
    public class Production : AggregrateRoot
    {
        public Guid AssignId { get; private set; }
        public Guid UserId { get; private set; }
        public int QuantitySend { get; private set; }
        public int QuantityReceive { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeOnly Time { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public Guid? ReworkRequestId { get; private set; }

        public Production(Guid id, Guid assignId, Guid userId, int quantitySend, Guid? reworkRequestId)
            : base(id)
        {
            AssignId = assignId;
            UserId = userId;
            QuantitySend = quantitySend;
            Date = DateOnly.FromDateTime(DateTime.Now);
            Time = TimeOnly.FromDateTime(DateTime.Now);
            Status = "PendingQC";
            ReworkRequestId = reworkRequestId;

            //AddDomainEvent(new ProductionCreatedEvent(assignId, userId, quantity));
        }

        private Production() : base(Guid.NewGuid()) { }

        public static Production Create(Guid assignId, Guid userId, int quantity, Guid? reworkRequestId)
        {
            return new Production(Guid.NewGuid(), assignId, userId, quantity, reworkRequestId);
        }

        public void SetQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            QuantityReceive = newQuantity;
        }

        public void MarkAsCompleted()
        {
            Status = "Passed";
        }

        public void PendingQC() => Status = "PendingQC";
        public void Rework() => Status = "Rework";
        public void CompleteWithLoss() => Status = "CompleteWithLoss";
    }
}
