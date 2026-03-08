using Domain.Events;
using Domain.Primitives;

namespace Domain.Entities
{
    public class Batch : AggregrateRoot
    {
        public Guid ProductId { get; private set; }
        public Guid? UserId { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal Quantity { get; private set; }
        public decimal ActualQuantity { get; private set; }
        public decimal LostQuantity { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Note { get; private set; }
        public bool isDeleted { get; private set; }

        //private readonly List<Assignment> _assignments = new();
        //public IReadOnlyCollection<Assignment> Assignments => _assignments.AsReadOnly();

        public ICollection<Assignment> Assignments { get; private set; } = new List<Assignment>();

        private readonly List<Evaluate> _evaluates = new();
        public IReadOnlyCollection<Evaluate> Evaluates => _evaluates.AsReadOnly();

        private readonly List<Production> _productions = new();
        public Product Product { get; private set; }

        public ICollection<Product> Products { get; private set; } = new List<Product>();

        //private readonly List<MaterialUse> _materialUses = new();
        //public IReadOnlyCollection<MaterialUse> Materials => _materialUses.AsReadOnly();

        public ICollection<MaterialUse> MaterialUses { get; private set; } = new List<MaterialUse>();

        public Batch(Guid Id, Guid productId, Guid? userId, string code, decimal quantity, DateOnly startDate, DateOnly endDate)
            : base(Id)
        {
            ProductId = productId;
            UserId = userId;
            Code = code;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            CreatedAt = DateTime.Now;
            Status = "Planned";
        }

        private Batch() : base(Guid.NewGuid()) { }

        public static Batch Create(Guid productId, Guid? userId, string code, decimal quantity, DateOnly startDate, DateOnly endDate)
        {
            return new Batch(Guid.NewGuid(), productId, userId, code, quantity, startDate, endDate);
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

        public void CompleteBatch(decimal completedQuantity, decimal rejectedQuantity, string? note = null)
        {
            this.ActualQuantity = completedQuantity;
            if (completedQuantity <= Quantity)
            {
                this.LostQuantity = Quantity - completedQuantity;
            }
            else
            {
                var extraQuantity = completedQuantity - Quantity;
                var quantityConflict = extraQuantity - rejectedQuantity;
                this.ActualQuantity = Quantity + quantityConflict;
                if (quantityConflict < 0)
                {
                    this.LostQuantity = Math.Abs(quantityConflict);
                }
            }
            UpdateStatus("Completed");
            Note = note;
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

        public bool ActiveNextAssignment(Guid assignTransferRequestId, Guid completedAssignmentId, decimal quantityCompleted, string? noteForFinal = null)
        {
            var currentAssignment = this.Assignments.FirstOrDefault(a => a.Id == completedAssignmentId);

            //nếu là xưởng khoán
            if (!currentAssignment.StepOrder.HasValue)
            {
                AddDomainEvent(new FinalTransferRequestCreatedEvent(
                    Guid.NewGuid(),
                    assignTransferRequestId,
                    quantityCompleted,
                    noteForFinal));

                currentAssignment.UpdateStatus("Completed");
                currentAssignment.UpdateDateComplete();

                return false;
            }

            var currentSteporder = currentAssignment.StepOrder;
            var nextAssignment = this.Assignments.Where(a => a.StepOrder > currentSteporder).OrderBy(a => a.StepOrder).FirstOrDefault();

            if (nextAssignment is not null)
            {
                nextAssignment.Active();
                AddDomainEvent(new AssignmentActivedEvent(Code, currentAssignment.WorkshopId, nextAssignment.StartDate, nextAssignment.WorkshopId));
                return true;
            }

            else
            {
                AddDomainEvent(new FinalTransferRequestCreatedEvent(
                    Guid.NewGuid(),
                    assignTransferRequestId,
                    quantityCompleted,
                    noteForFinal));

                //this.CompleteBatch(quantityCompleted, rejectedQuantity);
                currentAssignment.UpdateStatus("Completed");
                currentAssignment.UpdateDateComplete();
                return false;
            }
        }

        public void UpdateMaterialUsage(Guid assignmentId, Guid materialId, decimal reconciledQuantity, Guid userId)
        {
            var assignmentCurrent = this.Assignments.FirstOrDefault(a => a.Id == assignmentId);
            if (assignmentCurrent is null)
                throw new InvalidOperationException("Không tìm thấy công đoạn trong lô hàng.");

            if (assignmentCurrent.Status == "Reworking")
            {
                var currentMaterialUsage = this.MaterialUses.FirstOrDefault(m => m.AssignId == assignmentId && m.MaterialId == materialId && m.ReworkRequestId != null);
                if (currentMaterialUsage is null)
                    throw new InvalidOperationException("Không tìm thấy bản ghi sử dụng NVL cho công đoạn tái chế này.");

                if (reconciledQuantity > currentMaterialUsage.QuantityDivide + currentMaterialUsage.QuantityRequest)
                    throw new InvalidOperationException($"Số lượng NVL ghi nhận ({reconciledQuantity}) không được lớn hơn số lượng yêu cầu ({currentMaterialUsage.QuantityDivide + currentMaterialUsage.QuantityRequest}).");

                currentMaterialUsage.UpdateReconciledQuantity(reconciledQuantity);
                AddDomainEvent(new MaterialUsageReconciledEvent(Code, currentMaterialUsage.Id, userId, reconciledQuantity));
            }
            else
            {
                var currentMaterialUsage = this.MaterialUses.FirstOrDefault(m => m.AssignId == assignmentId && m.MaterialId == materialId && m.ReworkRequestId == null);
                if (currentMaterialUsage is null)
                    throw new InvalidOperationException("Không tìm thấy bản ghi sử dụng NVL cho công đoạn này.");

                if (reconciledQuantity > currentMaterialUsage.QuantityDivide + currentMaterialUsage.QuantityRequest)
                    throw new InvalidOperationException($"Số lượng NVL ghi nhận ({reconciledQuantity}) không được lớn hơn số lượng yêu cầu ({currentMaterialUsage.QuantityDivide + currentMaterialUsage.QuantityRequest}).");

                currentMaterialUsage.UpdateReconciledQuantity(reconciledQuantity);
                AddDomainEvent(new MaterialUsageReconciledEvent(Code, currentMaterialUsage.Id, userId, reconciledQuantity));
            }
        }

        public void UpdateLeadForBatch(Guid userId)
        {
            UserId = userId;
        }
    }
}
