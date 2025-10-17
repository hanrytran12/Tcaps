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
    }
}
