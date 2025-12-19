using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Workshop : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public int? StepOrder { get; private set; }
        public WorkshopType WorkshopType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Status { get; private set; }
        public Workshop(Guid id, string name, string description, int? stepOrder, WorkshopType workshopType)
            : base(id)
        {
            Name = name;
            Description = description;
            StepOrder = stepOrder;
            WorkshopType = workshopType;
            CreatedAt = DateTime.Now;
            Status = "UnAssigned";
        }

        public static Workshop Create(string name, string description, int? stepOrder, WorkshopType workshopType)
        {
            return new Workshop(Guid.NewGuid(), name, description, stepOrder, workshopType);
        }

        private Workshop() : base(Guid.NewGuid()) { }

        public void Insert(int stepOrder)
        {
            StepOrder = stepOrder;
        }

        public void IncreaseStepOrder() => StepOrder++;
        public void DecreaseStepOrder() => StepOrder--;
        public void MarkAssigned() => Status = "Assigned";

        public void Update(string? name, string? description)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;
        }

        public void SwapStepOrder(Workshop other)
        {
            if (StepOrder == null || other.StepOrder == null)
                throw new ArgumentNullException("Không thể swap khi StepOrder null");

            (StepOrder, other.StepOrder) = (other.StepOrder, StepOrder);
        }

    }
}
