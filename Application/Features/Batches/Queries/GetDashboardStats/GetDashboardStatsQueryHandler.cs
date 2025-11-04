using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardResultDTO>
    {
        private readonly IAppDbContext _context;
        public GetDashboardStatsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResultDTO> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Batches.AsNoTracking();
            query = query.Where(batch => batch.ProductId == request.ProductId && !batch.isDeleted);

            if (request.StartDate != null)
            {
                query = query.Where(query => query.StartDate >= request.StartDate);
            }

            if (request.EndDate != null)
            {
                query = query.Where(query => query.EndDate <= request.EndDate);
            }

            var stats = new DashboardStatsDTO
            {
                TotalBatches = await query.CountAsync(),
                InProgressBatches = await query.CountAsync(b => b.Status == "InProgress"),
                CompletedBatches = await query.CountAsync(b => b.Status == "Completed"),
            };

            var batchDetails = await query.Include(b => b.Assignments).Select(b => new
            {
                Batch = b,
                TotalAssignments = b.Assignments.Count(),
                CompletedAssignments = b.Assignments.Count(a => a.Status == "Completed")
            })
            .Select(data => new DashboardBatchDetailDTO
            {
                Code = data.Batch.Code,
                Quantity = data.Batch.Quantity,
                Status = data.Batch.Status,
                StartDate = data.Batch.StartDate,
                EndDate = data.Batch.EndDate,

                ProgressPercentage = (data.TotalAssignments > 0) ? Math.Round((double)data.CompletedAssignments / data.TotalAssignments * 100, 2) : 0,
                Assignments =
                (
                    from a in data.Batch.Assignments
                    join w in _context.Workshop on a.WorkshopId equals w.Id
                    select new DashboardAssignmentDTO
                    {
                        WorkshopName = w.Name,
                        Quantity = a.Quantity,
                        Status = a.Status,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                    }).ToList()
            }).AsNoTracking().ToListAsync();

            return new DashboardResultDTO
            {
                Stats = stats,
                Batches = batchDetails
            };
        }
    }
}
