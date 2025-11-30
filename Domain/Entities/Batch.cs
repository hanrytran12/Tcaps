using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Batch : AggregrateRoot
    {
        public Guid ProductId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal Quantity { get; private set; }
        public decimal ActualQuantity { get; private set; }
        public decimal LostQuantity { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
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

        public Batch(Guid Id, Guid productId, string code, decimal quantity, DateOnly startDate, DateOnly endDate)
            : base(Id)
        {
            ProductId = productId;
            Code = code;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.Now;
            Status = "Planned";
        }

        private Batch() : base(Guid.NewGuid()) { }

        public static Batch Create(Guid productId, string code, decimal quantity, DateOnly startDate, DateOnly endDate)
        {
            return new Batch(Guid.NewGuid(), productId, code, quantity, startDate, endDate);
        }

        public void MarkAsDeleted()
        {
            if (Status == "Completed")
            {
                throw new InvalidOperationException($"Cannot delete batch in completed.");
            }

            isDeleted = true;
            Status = "Cancelled";
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }

        public void CompleteBatch(decimal completedQuantity)
        {
            this.ActualQuantity = completedQuantity;
            this.LostQuantity = Quantity - completedQuantity;
            UpdateStatus("Completed");

            AddDomainEvent(new BatchCompletedEvent(Id, Code));
        }

        public void UpdateDetails(decimal quantity, DateOnly startDate, DateOnly endDate)
        {
            if (Status != "Planned")
            {
                throw new InvalidOperationException($"Cannot update batch in status: {Status}.");
            }

            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void AddAssignment(Assignment assignment)
        {
            if (Status == "Planned")
            {
                Status = "InProgress";
            }

            Assignments.Add(assignment);
        }

        public void NotifyPlanCreated()
        {
            var assignmentInfos = this.Assignments.Select(a => new AssignmentsInfo
            {
                WorkshopId = a.WorkshopId,
                ExpectedDeliveryDate = a.ExpectedDeliveryDate,
            }).ToList();

            AddDomainEvent(new AssignmentsPlannedEvent(this.Code, assignmentInfos));
        }

        public void ConfirmMaterialReceiptForAssignment(Guid assignmentId)
        {
            var assignmentToConfirm = this.Assignments.FirstOrDefault(a => a.Id == assignmentId);

            if (assignmentToConfirm is null)
            {
                throw new InvalidOperationException("Công đoạn không tồn tại trong lô hàng này.");
            }

            if (assignmentToConfirm.Status != "Planned")
            {
                return;
            }

            var firstStepOrderInPlan = this.Assignments.Min(a => a.StepOrder);
            bool isFirstStep = (assignmentToConfirm.StepOrder == firstStepOrderInPlan);
            assignmentToConfirm.UpdateWhenQcConfirmed(isFirstStep);
        }

        public void ActiveNextAssignment(Guid completedAssignmentId, decimal quantityCompleted)
        {
            var currentAssignment = this.Assignments.FirstOrDefault(a => a.Id == completedAssignmentId);

            //currentAssignment.UpdateStatus("Completed");

            var currentSteporder = currentAssignment.StepOrder;
            var nextAssignment = this.Assignments.Where(a => a.StepOrder > currentSteporder).OrderBy(a => a.StepOrder).FirstOrDefault();

            if (nextAssignment is not null)
            {
                nextAssignment.Active();
                AddDomainEvent(new AssignmentActivedEvent(Code, currentAssignment.WorkshopId, nextAssignment.StartDate, nextAssignment.WorkshopId));
            }

            else
            {
                this.CompleteBatch(quantityCompleted);
                currentAssignment.UpdateStatus("Completed");
                currentAssignment.UpdateDateComplete();
            }
        }

        public void UpdateMaterialUsage(Guid assignmentId, Guid materialId, decimal reconciledQuantity, Guid userId)
        {
            var assignmentCurrent = this.Assignments.FirstOrDefault(a => a.Id == assignmentId);

            if (assignmentCurrent.Status == "Reworking")
            {
                var currentMaterialUsage = this.MaterialUses.FirstOrDefault(m => m.AssignId == assignmentId && m.MaterialId == materialId && m.ReworkRequestId != null);
                if (reconciledQuantity > currentMaterialUsage.QuantityDivide)
                {
                    throw new InvalidOperationException("Số lượng ghi nhận không được lớn hơn số lượng yêu cầu.");
                }
                currentMaterialUsage.UpdateReconciledQuantity(reconciledQuantity);
                AddDomainEvent(new MaterialUsageReconciledEvent(Code, currentMaterialUsage.Id, userId, reconciledQuantity));
            }

            else
            {
                var currentMaterialUsage = this.MaterialUses.FirstOrDefault(m => m.AssignId == assignmentId && m.MaterialId == materialId);
                if (reconciledQuantity > currentMaterialUsage.QuantityDivide)
                {
                    throw new InvalidOperationException("Số lượng ghi nhận không được lớn hơn số lượng yêu cầu.");
                }
                currentMaterialUsage.UpdateReconciledQuantity(reconciledQuantity);
                AddDomainEvent(new MaterialUsageReconciledEvent(Code, currentMaterialUsage.Id, userId, reconciledQuantity));
            }
        }
    }
}
