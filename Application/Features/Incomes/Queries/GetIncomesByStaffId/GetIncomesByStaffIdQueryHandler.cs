using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Incomes.Queries.GetIncomesByStaffId
{
    public class GetIncomesByStaffIdQueryHandler : IRequestHandler<GetIncomesByStaffIdQuery, List<Application.DTOs.Response.IncomeHistoryDTO>>
    {
        private readonly IIncomeRepository _incomeRepository;

        public GetIncomesByStaffIdQueryHandler(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }
        public async Task<List<Application.DTOs.Response.IncomeHistoryDTO>> Handle(GetIncomesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var incomes = await _incomeRepository.GetIncomeHistoryAsync(request.StaffId);

            if (request.Date.HasValue)
            {
                incomes = incomes
                    .Where(i => DateOnly.FromDateTime(i.CreatedAt) == request.Date.Value)
                    .ToList();
            }

            return incomes
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new Application.DTOs.Response.IncomeHistoryDTO
                {
                    Id = i.Id,
                    BatchId = i.BatchId,
                    ProductionId = i.ProductionId,
                    UserId = i.UserId,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice,
                    CreatedAt = i.CreatedAt
                })
                .ToList();
        }
    }
}
