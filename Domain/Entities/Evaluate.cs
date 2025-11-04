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

        private readonly List<ComponentDefect> _componentDefects = new();
        public IReadOnlyCollection<ComponentDefect> ComponentDefects => _componentDefects.AsReadOnly();

        public Evaluate(Guid id, Guid productionId, Guid? userId, string note, int quantityError, string image, string status)
            : base(id)
        {
            ProductionId = productionId;
            UserId = userId;
            Note = note;
            QuantityError = quantityError;
            Image = image;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = status;

            //AddDomainEvent(new EvaluateCreatedEvent(Id, productionId, userId.Value, quantityError, note, status));
        }

        private Evaluate() : base(Guid.NewGuid()) { }

        public static Evaluate Create(Guid productionId, Guid userId, int quantityError, string note, string image, string status)
        {
            return new Evaluate(Guid.NewGuid(), productionId, userId, note, quantityError, image, status);
        }

        public void Update(string note, int quantityError, string image)
        {
            Note = note;
            QuantityError = quantityError;
            Image = image;
        }

        public void UpdateResolveComponent(Guid componentId, string status)
        {
            if (string.IsNullOrEmpty(status))
                throw new Exception("Status không được để trống.");

            var component = _componentDefects.FirstOrDefault(x => x.Id == componentId);
            if (component == null)
                throw new Exception($"Không tìm thấy ComponentDefect với Id = {componentId}");

            component.Resolve(status);
            AddDomainEvent(new ComponentResolvedEvent(componentId, component.EvaluateId, component.Quantity, status));
        }

        public void UpdateConfirmComponent(Guid componentId, string status)
        {
            if (string.IsNullOrEmpty(status))
                throw new Exception("Status không được để trống.");

            var component = _componentDefects.FirstOrDefault(x => x.Id == componentId);
            if (component == null)
                throw new Exception($"Không tìm thấy ComponentDefect với Id = {componentId}");

            component.Confirmed(status);
            AddDomainEvent(new ComponentConfirmEvent(componentId, component.EvaluateId, component.Quantity, status));
        }
    }
}
