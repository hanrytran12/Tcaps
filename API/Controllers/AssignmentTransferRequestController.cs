using Application.DTOs.Response;
using Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest;
using Application.Features.AssingmentTransferRequest.Commands.QcTransportReception;
using Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport;
using Application.Features.AssingmentTransferRequest.Queries.GetReconciliationSummary;
using Application.Features.AssingmentTransferRequest.Queries.GetTransferRequestByAssignmentId;
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

        [HttpGet]
        [Authorize(Roles = "Lead")]
        public async Task<ActionResult<List<TrasnferRequestDTO>>> GetAllTrasnferRequest()
        {
            var query = new GetAllTransferRequestQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{assignmentId:guid}/reconcilliation-summary")]
        public async Task<IActionResult> GetReconciliationSummary(Guid assignmentId)
        {
            var query = new GetReconciliationSummaryQuery(assignmentId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-assignment/{assignmentId:guid}")]
        public async Task<IActionResult> GetTransferRequestByAssignmentId(Guid assignmentId)
        {
            var query = new GetTransferRequestByAssignmentIdQuery(assignmentId);
            var result = await _mediator.Send(query);
            return Ok(result);
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
        [Authorize(Roles = "Lead,QCTransport")]
        public async Task<IActionResult> ApproveTrasnferRequest(Guid transferRequestId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var role = User.FindFirstValue(ClaimTypes.Role);
            var isQcTransport = User.FindFirstValue("isQcTransport");

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Chỉ cho phép nếu có claim isQcTransport = true
            if (string.Equals(role, "QCTransport", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(isQcTransport, "true", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }

            var command = new UpdateAssignmentTransferRequestCommand(transferRequestId, userId);
            var result = await _mediator.Send(command);
            return (result.IsSuccess) ? NoContent() : BadRequest(result.error);
        }

        [HttpGet("qc-transport")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> GetForQCTransport([FromQuery] GetAssignmentTransferForQcTransportQuery query)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var role = User.FindFirstValue(ClaimTypes.Role);
            var isQcTransport = User.FindFirstValue("isQcTransport");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out _))
            {
                return Unauthorized();
            }
            // Chỉ cho phép nếu có claim isQcTransport = true
            if (!string.Equals(isQcTransport, "true", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("qc-transport-reception")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> QCTransportReception([FromQuery] Guid assignmentTransferId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            //var role = User.FindFirstValue(ClaimTypes.Role);

            var isQcTransport = User.FindFirstValue("isQcTransport");

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            // Chỉ cho phép nếu có claim isQcTransport = true
            if (!string.Equals(isQcTransport, "true", StringComparison.OrdinalIgnoreCase))
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }

            var command = new QcTransportReceptionCommand
            {
                QCTransportId = userId,
                AssignmentTransferRequestId = assignmentTransferId
            };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
