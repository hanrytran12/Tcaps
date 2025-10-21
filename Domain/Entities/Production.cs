using Domain.Primitives;

namespace Domain.Entities
{
    public class Production : Entity
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
            Date = new DateOnly();
            Status = "Pending";
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

        public void MarkAsCompleted()
        {
            Status = "Completed";
        }

        public void Submit() => Status = "Submitted";
        public void Approve() => Status = "Approved";
        public void Reject() => Status = "Rejected";
    }
}
