using Application.DTOs.Response;
using Application.Features.Incomes.Queries.GetIncomesByStaffId;
using Application.Features.Incomes.Queries.GetMonthlyIncome;
using Application.Features.Incomes.Queries.GetTotalIncomeExpect;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public IncomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-staff")]
        public async Task<List<Income>> GetIncomesByStaffId([FromQuery] DateOnly? date)
        {
            var query = new GetIncomesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                Date = date
            };

            return await _mediator.Send(query);
        }

        [HttpGet("total-monthly")]
        public async Task<MonthlyIncomeDTO> GetMonthlyIncome([FromQuery] int month, [FromQuery] int year)
        {
            var query = new GetMonthlyIncomeQuery
            {
                StaffId = CurrentUserId,
                Month = month,
                Year = year
            };
            return await _mediator.Send(query);
        }

        [HttpGet("income-expected")]
        public async Task<IncomeExpectedDTO> GetIncomeExpected()
        {
            var query = new GetTotalIncomeExpectedQuery
            {
                StaffId = CurrentUserId
            };

            return await _mediator.Send(query);
        }
    }
}
