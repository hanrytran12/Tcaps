using Domain.Primitives;
using FluentValidation;

namespace Domain.Entities
{
    public class ComponentDefect : Entity
    {
        public Guid EvaluateId { get; private set; }
        public string DefectType { get; private set; } = string.Empty;
        public string Serverity { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Solution { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public ComponentDefect(Guid id, Guid evaluateId, string defectType, string serverity, string description, string solution, int quantity, string status)
            : base(id)
        {
            EvaluateId = evaluateId;
            DefectType = defectType;
            Serverity = serverity;
            Description = description;
            Solution = solution;
            Quantity = quantity;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
            Status = status;
        }

        private ComponentDefect() : base(Guid.NewGuid()) { }

        public static ComponentDefect Create(Guid evaluateId, string defectType, string serverity, string description, string solution, int quantity, string status)
        {
            return new ComponentDefect(Guid.NewGuid(), evaluateId, defectType, serverity, description, solution, quantity, status);
        }

        public void Update(string defectType, string severity, string description, string solution, int quantity, string status)
        {
            DefectType = defectType;
            Serverity = severity;
            Description = description;
            Solution = solution;
            Quantity = quantity;
            Status = status;
        }

        public void Rework() => Status = "Rework";
        public void Resolve(string status) => Status = status;
        public void Confirmed(string status) => Status = status;
        public void Unfixable() => Status = "Unfixable";
    }
}
