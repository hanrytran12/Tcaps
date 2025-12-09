using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Incomes.Queries.GetTotalIncomeExpect
{
    public class GetTotalIncomeExpectedQuery : IRequest<IncomeExpectedDTO>
    {
        public Guid StaffId { get; set; }
    }
}
