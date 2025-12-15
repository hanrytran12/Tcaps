using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId
{
    public class GetDetailAssignmentByBatchIdQueryHandler : IRequestHandler<GetDetailAssignmentByBatchIdQuery, List<DashboardAssignmentDTO>>
    {
        private readonly IAppDbContext _context;

        public GetDetailAssignmentByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<DashboardAssignmentDTO>> Handle(GetDetailAssignmentByBatchIdQuery request, CancellationToken cancellationToken)
        {
            var assignments = await (
                from a in _context.Assignments.AsNoTracking()
                where a.BatchId == request.BatchId
                join w in _context.Workshop.AsNoTracking() on a.WorkshopId equals w.Id
                orderby a.StepOrder
                select new DashboardAssignmentDTO
                {
                    WorkshopName = w.Name,
                    Quantity = a.Quantity,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Status = a.Status,
                }).ToListAsync(cancellationToken);

            return assignments;
        }
    }
}
