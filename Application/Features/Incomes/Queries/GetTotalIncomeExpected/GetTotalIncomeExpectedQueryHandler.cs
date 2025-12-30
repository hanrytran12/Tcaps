using Application.DTOs.Response;
using Application.Features.Incomes.Queries.GetTotalIncomeExpect;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Incomes.Queries.GetTotalIncomeExpected
{
    public class GetTotalIncomeExpectedQueryHandler : IRequestHandler<GetTotalIncomeExpectedQuery, IncomeExpectedDTO>
    {
        private readonly IAppDbContext _context;

        public GetTotalIncomeExpectedQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<IncomeExpectedDTO> Handle(GetTotalIncomeExpectedQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var result = from production in _context.Productions
                         join assign in _context.Assignments
                         on production.AssignId equals assign.Id
                         where production.UserId == request.StaffId &&
                                production.Date == today
                         select new
                         {
                             production.QuantityReceive,
                             assign.UnitPrice
                         };

            var totalQuantity = result.Sum(x => x.QuantityReceive);

            var totalIncomeExpected = result.Sum(x => x.QuantityReceive * x.UnitPrice);

            var dto = new IncomeExpectedDTO
            {
                QuantityExpect = totalQuantity,
                TotalExpect = totalIncomeExpected
            };

            return dto;
        }
    }
}
