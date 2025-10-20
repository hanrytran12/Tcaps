using Domain.Primitives;

namespace Domain.Entities
{
    public class Batch : AggregrateRoot
    {
        public Guid ProductId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal Quantity { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public bool isDeleted { get; private set; }

        private readonly List<Assignment> _assignments = new();
        public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

        private readonly List<Evaluate> _evaluates = new();
        public IReadOnlyCollection<Evaluate> Evaluates => _evaluates.AsReadOnly();

        private readonly List<Production> _productions = new();
        public IReadOnlyCollection<Production> Productions => _productions.AsReadOnly();

        private readonly List<MaterialUse> _materialUses = new();
        public IReadOnlyCollection<MaterialUse> Materials => _materialUses.AsReadOnly();

        public Batch(Guid Id, Guid productId, string code, decimal quantity, DateOnly startDate, DateOnly endDate)
            : base(Id)
        {
            ProductId = productId;
            Code = code;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            Status = "Pending";
        }

        private Batch() : base(Guid.NewGuid()) { }

        public void MarkAsDeleted()
        {
            isDeleted = true;
        }

        public void UpdateDetails(decimal quantity, DateOnly startDate, DateOnly endDate)
        {
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
