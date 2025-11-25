using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Incomes.Queries.GetTotalIncomeExpect
{
    public class GetTotalIncomeExpectedQuery : IRequest<Result<IncomeExpectedDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
