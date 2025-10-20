using Application.DTOs.Response;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetDashboardStats
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
            var batchListItemDTOs = batches.Select(b => new BatchListItemDTO
            {
                Code = b.Code,
                Quantity = b.Quantity,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Status = b.Status,
                ProgressPercentage = 0
            }).ToList();

            var totalBatches = batchListItemDTOs.Count;
            var inpogressBatches = batches.Count(b => b.Status == "Pending");
            var completedBatches = batches.Count(b => b.Status == "Completed");

            var statistics = new DashboardStatsDTO
            {
                TotalBatches = totalBatches,
                InProgressBatches = inpogressBatches,
                CompletedBatches = completedBatches
            };

            var result = new DashboardResultDTO
            {
                Stats = statistics,
                Batches = batchListItemDTOs
            };

            return result;
        }
    }
}
