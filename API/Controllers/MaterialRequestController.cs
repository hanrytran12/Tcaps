using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using Application.Features.MaterialRequest.Commands.CreateMaterialRequestFromQC;
using Application.Features.MaterialRequest.Commands.DispatchRequest;
using Application.Features.MaterialRequest.Commands.RejectMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequestForAdmin;
using Application.Features.MaterialRequest.Queries.GetMaterialRequestForQC;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetPendingRequestForQc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Features.MaterialRequest.Queries.GetMaterialRequestForQcTransport;
using Application.Features.MaterialRequest.Commands.QcTransportReceptionMaterialRequest;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MaterialRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequest()
        {
            var query = new GetAllMaterialRequestQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("pending-confirmation")]
        public async Task<ActionResult<List<PendingRequestDTO>>> GetPendingRequests()
        {
            var qcId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = new GetPendingRequestForQcQuery(qcId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("lead/admin/all-request")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMaterialRequestForAdminQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpGet("qc/request")]
        public async Task<IActionResult> GetByQCIdAsync([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetMaterialRequestForQCQuery
            {
                QcId = Guid.Parse(userIdString),
                Status = status
            };
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpPost("{assignmentId:guid}/dispatch-materials")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> DispatchMaterialsToAssignment(Guid assignmentId, [FromBody] List<MaterialRequestItemDTO> items)
        {
            var leadId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var command = new DispatchRequestCommand
            {
                AssignmentId = assignmentId,
                UserId = leadId,
                Items = items
            };

            var result = await _mediator.Send(command);

            return result.IsSuccess ? Ok() : BadRequest(result.error);
        }

        [HttpPost("qc/material-requests")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateMaterailRequestAsync([FromBody] CreateMaterialRequestFromQCCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            command.UserId = Guid.Parse(userIdString);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result);

            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpPut("approve/{id:guid}")]
        [Authorize(Policy = ("Lead"))]
        public async Task<IActionResult> ApproveMaterialRequest([FromRoute] Guid id)
        {
            var command = new Application.Features.MaterialRequest.Commands.ApproveRequestFromLead.ApproveRequestFromLeadCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpPut("confirmed/{id:guid}")]
        [Authorize(Policy = ("QC"))]
        public async Task<IActionResult> ConfirmMaterialRequest([FromRoute] Guid id, [FromBody] ConfirmRequestFromQcCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpPut("rejected/{id:guid}")]
        [Authorize(Policy = ("QC"))]
        public async Task<IActionResult> RejectMaterialRequest([FromRoute] Guid id, [FromBody] RejectMaterialRequestCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpGet("qc-transport")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> GetRequestsForQcTransport([FromQuery] GetMaterialRequestForQcTransportQuery query)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isQcTransport = User.FindFirstValue("isQcTransport");

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Chỉ cho phép nếu có claim isQcTransport = true
            if (role == "QCTransport" && isQcTransport?.ToLower() != "true")
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpPut("qc-transport-reception")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> QcTransportReceptionMaterialRequest([FromQuery] Guid materialRequestId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isQcTransport = User.FindFirstValue("isQcTransport");

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Chỉ cho phép nếu có claim isQcTransport = true
            if (role == "QCTransport" && isQcTransport?.ToLower() != "true")
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }

            var command = new QcTransportReceptionMaterialRequestCommand
            {
                QcTransportId = Guid.Parse(userIdString),
                MaterialRequestId = materialRequestId
            };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }
    }
}
