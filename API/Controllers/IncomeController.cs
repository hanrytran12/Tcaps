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
    public class IncomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IncomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("by-staff")]
        public async Task<IActionResult> GetIncomesByStaffId([FromQuery] DateOnly? date)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var staffId))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var query = new GetIncomesByStaffIdQuery
            {
                StaffId = staffId,
                Date = date
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("total-monthly")]
        public async Task<IActionResult> GetMonthlyIncome([FromQuery] int month, [FromQuery] int year)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var staffId))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }
            var query = new GetMonthlyIncomeQuery
            {
                StaffId = staffId,
                Month = month,
                Year = year
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("income-expected")]
        public async Task<IActionResult> GetIncomeExpected()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var staffId))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var query = new GetTotalIncomeExpectedQuery
            {
                StaffId = staffId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
