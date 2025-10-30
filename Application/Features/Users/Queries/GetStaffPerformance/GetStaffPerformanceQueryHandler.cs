using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetStaffPerformance
{
    public class GetStaffPerformanceQueryHandler : IRequestHandler<GetStaffPerformanceQuery, List<StaffPerformanceDTO>>
    {
        private readonly IAppDbContext _context;

        public GetStaffPerformanceQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StaffPerformanceDTO>> Handle(GetStaffPerformanceQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users
                .Where(u => u.Role == "Staff")
                .AsQueryable();

            if (request.WorkshopId.HasValue && request.WorkshopId.Value != Guid.Empty)
            {
                query = query.Where(user => user.WorkshopId == request.WorkshopId.Value);
            }

            var incomeStats = _context.Incomes
                .GroupBy(x => x.UserId)
                .Select(g => new StaffPerformanceDTO
                {
                    UserId = g.Key,
                    TotalIncome = g.Sum(i => (decimal?)i.TotalPrice) ?? 0,
                    TotalQuantitySold = g.Sum(i => (int?)i.Quantity) ?? 0
                });

            var finalQuery = from user in query
                             join income in _context.Incomes on user.Id equals income.UserId into userIncomes
                             from ui in userIncomes.DefaultIfEmpty()
                             group ui by new { user.Id, user.FullName } into g
                             select new StaffPerformanceDTO
                             {
                                 UserId = g.Key.Id,
                                 FullName = g.Key.FullName,
                                 TotalIncome = g.Sum(x => x != null ? x.TotalPrice : 0),
                                 TotalQuantitySold = (int)g.Sum(x => x != null ? x.Quantity : 0)
                             };

            return await finalQuery.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
