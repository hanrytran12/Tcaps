using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId
{
    public class GetDetailAssignmentByBatchIdQueryHandler : IRequestHandler<GetDetailAssignmentByBatchIdQuery, Result<List<DashboardAssignmentDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetDetailAssignmentByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<DashboardAssignmentDTO>>> Handle(GetDetailAssignmentByBatchIdQuery request, CancellationToken cancellationToken)
        {
            var assignments = await (from b in _context.Batches.AsNoTracking()
                                     join a in _context.Assignments.AsNoTracking() on b.Id equals a.BatchId
                                     join w in _context.Workshop.AsNoTracking() on a.WorkshopId equals w.Id
                                     where b.Id == request.BatchId
                                     select new DashboardAssignmentDTO
                                     {
                                         WorkshopName = w.Name,
                                         Quantity = a.Quantity,
                                         StartDate = a.StartDate,
                                         EndDate = a.EndDate,
                                         Status = a.Status
                                     }).ToListAsync(cancellationToken);

            return Result<List<DashboardAssignmentDTO>>.Success(assignments);
        }
    }
}
