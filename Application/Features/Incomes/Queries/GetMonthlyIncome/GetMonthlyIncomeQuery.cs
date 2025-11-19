using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Incomes.Queries.GetMonthlyIncome
{
    public class GetMonthlyIncomeQuery : IRequest<Result<MonthlyIncomeDTO>>
    {
        [JsonIgnore]
        public Guid StaffId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
