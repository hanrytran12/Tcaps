using Domain.Primitives;

namespace Domain.Entities
{
    public class Evaluate : Entity
    {
        public Guid ProductionId { get; private set; }
        public Guid? UserId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public Evaluate(Guid id, Guid productionId, Guid? userId, string note, string image)
            : base(id)
        {
            ProductionId = productionId;
            UserId = userId;
            Note = note;
            Image = image;
            CreatedAt = new DateOnly();
            Status = "Pending";
        }

        private Evaluate() : base(Guid.NewGuid()) { }
    }
}
