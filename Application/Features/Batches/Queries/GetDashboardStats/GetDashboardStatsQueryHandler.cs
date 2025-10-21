using Application.DTOs.Response;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Queries.GetDashboardStats
{
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardResultDTO>
    {
        private readonly IBatchRepository _batchRepository;
        public GetDashboardStatsQueryHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }
        public async Task<DashboardResultDTO> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year;
            DateOnly targetDate;
            try
            {
                targetDate = new DateOnly(year, request.Month, request.Day);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException("Invalid day or month provided.");
            }

            var batches = await _batchRepository.SearchAsync(request.Id, targetDate);

            var stats = new DashboardStatsDTO
            {
                TotalBatches = batches.Count(),
                InProgressBatches = batches.Count(b => b.Status == "Pending"),
                CompletedBatches = batches.Count(b => b.Status == "Completed"),
            };

            var batchDetails = batches.Select(b =>
            {
                var assignments = b.Assignments ?? new List<Assignment>();
                int completedAssignments = assignments.Count(a => a.Status == "Completed");
                if (completedAssignments > 15)
                {
                    completedAssignments = 15;
                }
                double progressPercentage = (assignments.Any())
                ? Math.Round((double)completedAssignments / 15 * 100, 2)
                : 0;


                return new DashboardBatchDetailDTO
                {
                    Code = b.Code,
                    Quantity = b.Quantity,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status,
                    ProgressPercentage = progressPercentage,
                    Assignments = assignments.Select(a => new DashboardAssignmentDTO
                    {
                        WorkshopId = a.WorkshopId,
                        Quantity = a.Quantity,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        Status = a.Status
                    }).ToList()
                };
            });

            var result = new DashboardResultDTO
            {
                Stats = stats,
                Batches = batchDetails
            };

            return result;
        }
    }
}
