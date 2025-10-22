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

            var finalQuery = query.Select(u => new StaffPerformanceDTO
            {
                UserId = u.Id,
                FullName = u.FullName,
                TotalIncome = (from order in _context.Incomes
                               where order.UserId == u.Id
                               select order.TotalPrice).Sum(),
                TotalQuantitySold = (from order in _context.Incomes
                                     where order.UserId == u.Id
                                     select order.Quantity).Sum()
            });

            return await finalQuery.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
