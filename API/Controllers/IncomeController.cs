using Application.DTOs.Response;
using Application.Features.Incomes.Queries.GetIncomesByStaffId;
using Application.Features.Incomes.Queries.GetMonthlyIncome;
using Application.Features.Incomes.Queries.GetTotalIncomeExpect;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : BaseApiController
    {
        public IncomeController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("by-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<IncomeHistoryDTO>>> GetIncomesByStaffId([FromQuery] DateOnly? date)
        {
            var result = await Mediator.Send(new GetIncomesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                Date = date
            });
            return Ok(result);
        }

        [HttpGet("total-monthly")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<MonthlyIncomeDTO>> GetMonthlyIncome([FromQuery] int month, [FromQuery] int year)
        {
            var result = await Mediator.Send(new GetMonthlyIncomeQuery
            {
                StaffId = CurrentUserId,
                Month = month,
                Year = year
            });
            return Ok(result);
        }

        [HttpGet("income-expected")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<IncomeExpectedDTO>> GetIncomeExpected()
        {
            var result = await Mediator.Send(new GetTotalIncomeExpectedQuery
            {
                StaffId = CurrentUserId
            });
            return Ok(result);
        }
    }
}
