using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromLead;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using Application.Features.MaterialRequest.Commands.CreateMaterialRequestFromQC;
using Application.Features.MaterialRequest.Commands.DispatchRequest;
using Application.Features.MaterialRequest.Commands.QcTransportReceptionMaterialRequest;
using Application.Features.MaterialRequest.Commands.RejectMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequestForAdmin;
using Application.Features.MaterialRequest.Queries.GetMaterialRequestForQC;
using Application.Features.MaterialRequest.Queries.GetMaterialRequestForQcTransport;
using Application.Features.MaterialRequest.Queries.GetPendingRequestForQc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialRequestController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRequest()
        {
            var query = new GetAllMaterialRequestQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("pending-confirmation")]
        public async Task<ActionResult<List<PendingRequestDTO>>> GetPendingRequests()
        {
            var query = new GetPendingRequestForQcQuery(CurrentUserId);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("lead/admin/all-request")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<Result<List<MaterialRequestDTO>>> GetAllAsync([FromQuery] GetAllMaterialRequestForAdminQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("qc/request")]
        public async Task<Result<List<MaterialRequestDTO>>> GetByQCIdAsync([FromQuery] string? status)
        {
            var query = new GetMaterialRequestForQCQuery
            {
                QcId = CurrentUserId,
                Status = status
            };
            return await Mediator.Send(query);
        }

        [HttpGet("qc-transport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<Result<MaterialRequestDTO>> GetRequestsForQcTransport([FromQuery] GetMaterialRequestForQcTransportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("{assignmentId:guid}/dispatch-materials")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> DispatchMaterialsToAssignment(Guid assignmentId, [FromBody] List<MaterialRequestItemDTO> items)
        {
            var command = new DispatchRequestCommand
            {
                AssignmentId = assignmentId,
                UserId = CurrentUserId,
                Items = items
            };

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPost("qc/material-requests")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateMaterailRequestAsync([FromBody] CreateMaterialRequestFromQCCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("approve/{id:guid}")]
        [Authorize(Policy = ("Lead"))]
        public async Task<IActionResult> ApproveMaterialRequest([FromRoute] Guid id)
        {
            var command = new Application.Features.MaterialRequest.Commands.ApproveRequestFromLead.ApproveRequestFromLeadCommand { Id = id };
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpPut("confirmed/{id:guid}")]
        [Authorize(Roles = "QC,Lead")]
        public async Task<IActionResult> ConfirmMaterialRequest([FromRoute] Guid id, [FromBody] ConfirmRequestFromQcCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
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
            var result = await Mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpPut("qc-transport-reception")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> QcTransportReceptionMaterialRequest([FromQuery] Guid materialRequestId)
        {
            var command = new QcTransportReceptionMaterialRequestCommand
            {
                QcTransportId = CurrentUserId,
                MaterialRequestId = materialRequestId
            };
            var result = await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("lead-confirm")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> LeadConfirm([FromQuery] ConfirmRequestFromLeadCommand command)
        {
            var result = await Mediator.Send(command);
            return NoContent();
        }
    }
}
