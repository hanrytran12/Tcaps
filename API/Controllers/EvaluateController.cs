using System.Security.Claims;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Evaluates.Commands.AddEvaluate;
using Application.Features.Evaluates.Commands.UpdateEvaluate;
using Application.Features.Evaluates.Queries.GetAllEvaluate;
using Application.Features.Evaluates.Queries.GetEvaluatesByQCId;
using Application.Features.Evaluates.Queries.GetEvaluatesByStaffId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EvaluateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllEvaluateQuery());
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetByQCId([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetEvaluatesByQCIdQuery
            {
                QC_Id = Guid.Parse(userIdString),
                Status = status
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-staff")]
        public async Task<IActionResult> GetByStaffId([FromQuery] Guid assignId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetEvaluatesByStaffIdQuery
            {
                StaffId = Guid.Parse(userIdString),
                AssignId = assignId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateEvaluate([FromBody] AddEvaluateCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            command.UserId = Guid.Parse(userIdString);
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPut("{evaluateId:guid}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateEvaluate(Guid evaluateId, [FromBody] UpdateEvaluateCommand command)
        {
            command.Id = evaluateId;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(new {success = false, message = result.Error ?? "Update evaluate failed" });
        }
    }
}
