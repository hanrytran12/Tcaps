using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Batch : AggregrateRoot
    {
        public Guid ProductId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal Quantity { get; private set; }
        public string ImageURL { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public bool isDeleted { get; private set; }

        //private readonly List<Assignment> _assignments = new();
        //public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();

        private readonly List<Evaluate> _evaluates = new();
        public IReadOnlyCollection<Evaluate> Evaluates => _evaluates.AsReadOnly();

        private readonly List<Production> _productions = new();
        public IReadOnlyCollection<Production> Productions => _productions.AsReadOnly();

        //private readonly List<MaterialUse> _materialUses = new();
        //public IReadOnlyCollection<MaterialUse> Materials => _materialUses.AsReadOnly();

        public ICollection<MaterialUse> MaterialUses { get; private set; } = new List<MaterialUse>();

        public Batch(Guid Id, Guid productId, string code, decimal quantity, string imageURL, DateOnly startDate, DateOnly endDate)
            : base(Id)
        {
            ProductId = productId;
            Code = code;
            Quantity = quantity;
            ImageURL = imageURL;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            Status = "Pending";
        }

        private Batch() : base(Guid.NewGuid()) { }

        public static Batch Create(Guid productId, string code, decimal quantity, string imageURL, DateOnly startDate, DateOnly endDate)
        {
            return new Batch(Guid.NewGuid(), productId, code, quantity, imageURL, startDate, endDate);
        }

        public void MarkAsDeleted()
        {
            if (Status != "Pending")
            {
                throw new InvalidOperationException($"Cannot delete batch in status: {Status}.");
            }

            isDeleted = true;
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void UpdateDetails(decimal quantity, DateOnly startDate, DateOnly endDate)
        {
            if (Status != "Pending")
            {
                throw new InvalidOperationException($"Cannot update batch in status: {Status}.");
            }

            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void AddAssignment(Assignment assignment)
        {
            Assignments.Add(assignment);

            AddDomainEvent(new AssignmentAddedEvent(Code, assignment.WorkshopId, assignment.ExpectedDeliveryDate));
        }

        public void UpdateAssignmentsStatus(Guid assignmentId, string newStatus)
        {
            var assignment = Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (assignment is not null)
            {
                assignment.UpdateStatus(newStatus);
            }

            CheckForCompletion();
        }

        private void CheckForCompletion()
        {
            bool has15Assignments = Assignments.Count == 15;
            bool allAssignmentsCompleted = has15Assignments && Assignments.All(a => a.Status == "Completed");
            if (allAssignmentsCompleted)
            {
                UpdateStatus("Completed");
                AddDomainEvent(new BatchCompletedEvent(this.Id, this.Code));
            }
        }

        public void AddMaterialUse(MaterialUse materialUse)
        {
            MaterialUses.Add(materialUse);

            AddDomainEvent(new MaterialUseAddedEvent(materialUse.MaterialId, materialUse.BatchId, materialUse.AssignId, materialUse.QuantityDivide));
        }
    }
}
