using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.PlanAssignments
{
    public class PlanAssignmentsCommandHandler : IRequestHandler<PlanAssignmentsCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;

        public PlanAssignmentsCommandHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(PlanAssignmentsCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.BatchId);

            if (batch is null)
            {
                return Result.Failure("Không tồn tại lô hàng này");
            }

            if (batch.Status != "Planned")
            {
                return Result.Failure("Lô hàng đã phân công giai đoạn từ trước");
            }

            if (!request.PlanItems.Any())
            {
                return Result.Failure("Danh sách không được trống");
            }

            var sortedPlan = request.PlanItems.OrderBy(p => p.StepOrder).ToList();
            //foreach (var plan in sortedPlan)
            //{
            //    var assignment = Assignment.Create(request.BatchId, plan.WorkshopId, plan.StepOrder, plan.Quantity, plan.StartDate, plan.EndDate, plan.ExpectedDeliveryDate, plan.UnitPrice);
            //    batch.AddAssignment(assignment);
            //}
            for (int i = 0; i < sortedPlan.Count; i++)
            {
                var item = sortedPlan[i];
                var assignment = Assignment.Create(request.BatchId, item.WorkshopId, item.StepOrder, item.Quantity, item.StartDate, item.EndDate, item.ExpectedDeliveryDate, item.UnitPrice, item.RequiresMaterialDelivery);

                if (i == 0)
                {
                    if (assignment.RequiresMaterialDelivery == false)
                    {
                        assignment.Active();
                    }
                }

                batch.AddAssignment(assignment);
            }

            return Result.Success();
        }
    }
}
