using Domain.Primitives;

namespace Domain.Entities
{
    public class Workshop : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int StepOrder { get; private set; }

        public Workshop(Guid id, string name, string description, int stepOrder)
            : base(id)
        {
            Name = name;
            Description = description;
            StepOrder = stepOrder;
        }

        public static Workshop Create(string name, string description, int stepOrder)
        {
            return new Workshop(Guid.NewGuid(), name, description, stepOrder);
        }

        private Workshop() : base(Guid.NewGuid()) { }
    }
}
