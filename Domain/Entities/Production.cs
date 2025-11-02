using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Production : AggregrateRoot
    {
        public Guid AssignId { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; private set; }
        public DateOnly Date { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public Production(Guid id, Guid assignId, Guid userId, int quantity)
            : base(id)
        {
            AssignId = assignId;
            UserId = userId;
            Quantity = quantity;
            Date = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = "PendingQC";

            AddDomainEvent(new ProductionCreatedEvent(assignId, userId, quantity));
        }

        private Production() : base(Guid.NewGuid()) { }

        public void IncreaseQuantity()
        {
            Quantity += 1;
        }

        public void DecreaseQuantity()
        {
            if (Quantity <= 0)
                throw new InvalidOperationException("Quantity cannot be negative.");

            Quantity -= 1;
        }

        public void SetQuantity(int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            Quantity = newQuantity;
        }

        public void ReduceQuantity(int quantityError)
        {
            if (quantityError <= 0)
                return;

            if (Quantity < quantityError)
                throw new InvalidOperationException("Không thể giảm số lượng vượt quá số lượng hiện tại.");

            Quantity -= quantityError;
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
