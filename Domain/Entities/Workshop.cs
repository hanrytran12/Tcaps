using Domain.Primitives;

namespace Domain.Entities
{
    public class Workshop : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int StepOrder { get; private set; }

        public Workshop(Guid id, string name, string description)
            : base(id)
        {
            Name = name;
            Description = description;
        }

        private Workshop() : base(Guid.NewGuid()) { }
    }
}
