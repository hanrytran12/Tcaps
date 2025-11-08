using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using Application.Features.MaterialRequest.Commands.DispatchRequest;
using Application.Features.MaterialRequest.Commands.RejectMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetAllMaterialRequest;
using Application.Features.MaterialRequest.Queries.GetPendingRequestForQc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    }
}
