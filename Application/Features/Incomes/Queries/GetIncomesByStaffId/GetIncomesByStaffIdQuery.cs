using Domain.Entities;
using MediatR;

namespace Application.Features.Incomes.Queries.GetIncomesByStaffId
{
    public class GetIncomesByStaffIdQuery : IRequest<List<Application.DTOs.Response.IncomeHistoryDTO>>
    {
        public Guid StaffId { get; set; }
        public DateOnly? Date { get; set; }
    }
}
