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
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public ComponentDefect(Guid id, Guid evaluateId, string defectType, string serverity, string description, string solution, string status)
            : base(id)
        {
            EvaluateId = evaluateId;
            DefectType = defectType;
            Serverity = serverity;
            Description = description;
            Solution = solution;
            CreatedAt = new DateOnly();
            Status = status;
        }

        private ComponentDefect() : base(Guid.NewGuid()) { }

        public static ComponentDefect Create(Guid evaluateId, string defectType, string serverity, string description, string solution, string status)
        {
            return new ComponentDefect(Guid.NewGuid(), evaluateId, defectType, serverity, description, solution, status);
        }

        public void Update(string defectType, string severity, string description, string solution)
        {
            DefectType = defectType;
            Serverity = severity;
            Description = description;
            Solution = solution;
        }
    }
}
