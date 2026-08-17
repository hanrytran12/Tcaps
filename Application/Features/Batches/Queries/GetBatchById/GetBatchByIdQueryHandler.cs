using Application.Common.Exceptions;
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
                LeadName = _appDbContext.Users
                    .Where(u => u.Id == b.UserId)
                    .Select(u => u.FullName)
                    .FirstOrDefault(),

                TotalAssignments = b.Assignments.Count(),
                CompletedAssignments = b.Assignments.Count(a => a.Status == "Completed")
            })
            .Select(data => new BatchDetailResponseDTO
            {
                UserId = data.Batch.UserId ?? Guid.Empty,
                LeadName = data.LeadName ?? string.Empty,
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
                    join w in _appDbContext.Workshops on a.WorkshopId equals w.Id
                    orderby a.StepOrder
                    select new DashboardAssignmentDTO
                    {
                        AssignmentId = a.Id,
                        UnitPrice = a.UnitPrice,
                        WorkshopName = w.Name,
                        Quantity = a.Quantity,
                        Status = a.Status,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        ExpectedDeliveryDate = a.ExpectedDeliveryDate,
                    }).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();

            if (batchDetails == null)
            {
                throw new NotFoundException("Batch not found");
            }

            return batchDetails;
        }
    }
}
