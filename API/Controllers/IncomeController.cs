using System.Security.Claims;
using Application.Features.Incomes.Command.AddIncome;
using Application.Features.Incomes.Queries.GetIncomesByStaffId;
using Application.Features.Incomes.Queries.GetMonthlyIncome;
using Application.Features.Incomes.Queries.GetTotalIncomeExpect;
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
        public async Task<IActionResult> GetIncomesByStaffId([FromQuery] DateOnly? date)
        {
            var query = new GetIncomesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                Date = date
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("total-monthly")]
        public async Task<IActionResult> GetMonthlyIncome([FromQuery] int month, [FromQuery] int year)
        {
            var query = new GetMonthlyIncomeQuery
            {
                StaffId = CurrentUserId,
                Month = month,
                Year = year
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("income-expected")]
        public async Task<IActionResult> GetIncomeExpected()
        {
            var query = new GetTotalIncomeExpectedQuery
            {
                StaffId = CurrentUserId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
