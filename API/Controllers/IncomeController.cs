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
        public async Task<List<Income>> GetIncomesByStaffId([FromQuery] DateOnly? date)
        {
            return await Mediator.Send(new GetIncomesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                Date = date
            });
        }

        [HttpGet("total-monthly")]
        [Authorize(Roles = "Staff")]
        public async Task<MonthlyIncomeDTO> GetMonthlyIncome([FromQuery] int month, [FromQuery] int year)
        {
            return await Mediator.Send(new GetMonthlyIncomeQuery
            {
                StaffId = CurrentUserId,
                Month = month,
                Year = year
            });
        }

        [HttpGet("income-expected")]
        [Authorize(Roles = "Staff")]
        public async Task<IncomeExpectedDTO> GetIncomeExpected()
        {
            return await Mediator.Send(new GetTotalIncomeExpectedQuery
            {
                StaffId = CurrentUserId
            });
        }
    }
}
