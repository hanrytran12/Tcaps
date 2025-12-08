using Application.Common;
using Application.Common.Exceptions;
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
                throw new NotFoundException("Không tồn tại lô hàng này");
            }

            if (batch.Status != "Planned")
            {
                throw new ConflictException("Lô hàng đã phân công giai đoạn từ trước");
            }

            if (!request.PlanItems.Any())
            {
                throw new BadRequestException("Danh sách không được trống");
            }

            var sortedPlan = request.PlanItems.OrderBy(p => p.StepOrder).ToList();
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
            batch.NotifyPlanCreated();
            return Result.Success();
        }
    }
}
