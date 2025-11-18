using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTransferRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskTransferRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllTaskTransferRequestQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-QcTransport")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> GetByQcTransportAsync([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetTaskTransferRequestByQCTransportIdQuery
            {
                QcTransportId = Guid.Parse(userIdString),
                Status = status
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("for-lead")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskTransferRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }

        [HttpPut("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequestAsync([FromQuery] UpdateApproveTaskTransferRequestCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
