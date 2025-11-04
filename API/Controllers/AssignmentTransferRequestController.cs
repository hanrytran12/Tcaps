using Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest;
using Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentTransferRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AssignmentTransferRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] AddAssignmentTransferRequestCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var userId = Guid.Parse(userIdString);
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return (result.IsSuccess) ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("approved/{transferRequestId:guid}")]
        public async Task<IActionResult> ApproveTrasnferRequest(Guid transferRequestId)
        {
            var command = new UpdateAssignmentTransferRequestCommand(transferRequestId);
            var result = await _mediator.Send(command);
            return (result.IsSuccess) ? NoContent() : BadRequest(result.error);
        }
    }
}
