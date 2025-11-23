using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchById
{
    public class GetBatchByIdQueryHandler : IRequestHandler<GetBatchByIdQuery, BatchDetailResponseDTO>
    {
        private readonly IAppDbContext _appDbContext;
        public GetBatchByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BatchDetailResponseDTO> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _appDbContext.Batches.AsNoTracking();
            query = query.Where(b => b.Id == request.BatchId && !b.isDeleted);

            var totalIncome = await _appDbContext.Incomes.Where(i => i.BatchId == request.BatchId).SumAsync(i => i.TotalPrice);

            var batchDetails = await query.Include(b => b.Assignments).Select(b => new
            {
                Batch = b,
                TotalAssignments = b.Assignments.Count(),
                CompletedAssignments = b.Assignments.Count(a => a.Status == "Completed")
            })
            .Select(data => new BatchDetailResponseDTO
            {
                Code = data.Batch.Code,
                Quantity = data.Batch.Quantity,
                Status = data.Batch.Status,
                DurationDay = data.Batch.EndDate.DayNumber - data.Batch.StartDate.DayNumber,
                EndDate = data.Batch.EndDate,
                TotalPrice = (decimal)totalIncome,
                ActualQuantity = data.Batch.ActualQuantity,
                LostQuantity = data.Batch.LostQuantity,

                ProgressPercentage = (data.TotalAssignments > 0) ? Math.Round((double)data.CompletedAssignments / data.TotalAssignments * 100, 2) : 0,
                Assignments =
                (
                    from a in data.Batch.Assignments
                    join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                    select new DashboardAssignmentDTO
                    {
                        WorkshopName = w.Name,
                        Quantity = a.Quantity,
                        Status = a.Status,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                    }).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();

            return batchDetails;
        }
    }
}
