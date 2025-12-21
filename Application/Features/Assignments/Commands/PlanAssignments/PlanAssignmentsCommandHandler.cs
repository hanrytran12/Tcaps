using Application.Common;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.PlanAssignments
{
    public class PlanAssignmentsCommandHandler : IRequestHandler<PlanAssignmentsCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IMediator _mediator;

        public PlanAssignmentsCommandHandler(IBatchRepository batchRepository, IWorkshopRepository workshopRepository, IMediator mediator)
        {
            _batchRepository = batchRepository;
            _workshopRepository = workshopRepository;
            _mediator = mediator;
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

            var sortedPlan = request.PlanItems.ToList();
            int step = 1;

            var updatedWorkshops = new HashSet<Guid>();
            for (int i = 0; i < sortedPlan.Count; i++)
            {
                var item = sortedPlan[i];

                var workshop = await _workshopRepository.GetByIdAsync(item.WorkshopId);
                if (workshop is null)
                {
                    throw new NotFoundException("Xưởng này không tồn tại.");
                }

                if (updatedWorkshops.Add(workshop.Id))
                {
                    workshop.MarkAssigned();
                }

                int? stepOrder = null;
                if (workshop.WorkshopType != Domain.Enums.WorkshopType.Outsource)
                {
                    stepOrder = step++;
                }

                var assignment = Assignment.Create(
                    request.BatchId, 
                    item.WorkshopId, 
                    stepOrder, 
                    item.Quantity, 
                    item.StartDate, 
                    item.EndDate, 
                    item.ExpectedDeliveryDate, 
                    item.UnitPrice, 
                    item.RequiresMaterialDelivery);
                
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

            await _mediator.Publish(new AssignWorkshopEvent(batch.UserId ?? Guid.Empty, batch.Code));
            return Result.Success();
        }
    }
}
