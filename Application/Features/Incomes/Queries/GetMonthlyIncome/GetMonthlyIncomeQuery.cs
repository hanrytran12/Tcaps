using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Incomes.Queries.GetMonthlyIncome
{
    public class GetMonthlyIncomeQuery : IRequest<MonthlyIncomeDTO>
    {
        [JsonIgnore]
        public Guid StaffId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
