using Domain.Primitives;

namespace Domain.Entities
{
    public class ComponentDefect : Entity
    {
        public Guid EvaluateId { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public ComponentDefect(Guid id, Guid evaluateId, string description, int quantity, string status)
            : base(id)
        {
            EvaluateId = evaluateId;
            Description = description;
            Quantity = quantity;
            CreatedAt = DateTime.Now;
            Status = status;
        }

        private ComponentDefect() : base(Guid.NewGuid()) { }

        public static ComponentDefect Create(Guid evaluateId, string description, int quantity, string status)
        {
            return new ComponentDefect(Guid.NewGuid(), evaluateId, description, quantity, status);
        }

        public void Update(string description, int quantity, string status)
        {
            Description = description;
            Quantity = quantity;
            Status = status;
        }

        public void Rework() => Status = "Rework";
        public void Resolve(string status) => Status = status;
        public void Confirmed(string status) => Status = status;
        public void Unfixable() => Status = "Unfixabled";
    }
}
