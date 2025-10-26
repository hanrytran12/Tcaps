using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Evaluate : AggregrateRoot
    {
        public Guid ProductionId { get; private set; }
        public Guid? UserId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public int QuantityError { get; private set; }
        public string Image { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }

        public Evaluate(Guid id, Guid productionId, Guid? userId, string note, int quantityError, string image)
            : base(id)
        {
            ProductionId = productionId;
            UserId = userId;
            Note = note;
            QuantityError = quantityError;
            Image = image;
            CreatedAt = new DateOnly();
            Status = "Pending";

            AddDomainEvent(new EvaluateCreatedEvent(Id, productionId, userId.Value, quantityError, note));
        }

        private Evaluate() : base(Guid.NewGuid()) { }

        public static Evaluate Create(Guid productionId, Guid userId, int quantityError, string note, string image)
        {
            return new Evaluate(Guid.NewGuid(), productionId, userId, note, quantityError, image);
        }

        public void Update(string note, int quantityError, string image)
        {
            Note = note;
            QuantityError = quantityError;
            Image = image;
        }
    }
}
