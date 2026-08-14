using Application.DTOs.Response;
using Application.Features.ReworkRequest.Commands.ApproveReworkRequest;
using Application.Features.ReworkRequest.Commands.CreateReworkRequest;
using Application.Features.ReworkRequest.Commands.RejectReworkRequest;
using Application.Features.ReworkRequest.Queries.GetAllReworkRequest;
using Application.Features.ReworkRequest.Queries.GetRequestById;
using Application.Features.ReworkRequest.Queries.GetReworkByAssignId;
using Application.Features.ReworkRequest.Queries.GetReworkByQcId;
using Application.Features.ReworkRequest.Queries.GetReworkForDashboard;
using Application.Features.ReworkRequest.Queries.GetReworkReconciliationSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReworkRequestController : BaseApiController
    {
        public ReworkRequestController(ISender mediator) : base(mediator)
        {
        }
        [HttpGet]
        [Authorize(Policy = "Lead")]
        public async Task<ActionResult<List<ReworkRequestDTO>>> GetAllReworkRequest()
        {
            var result = await Mediator.Send(new GetAllReworkRequestQuery());
            return Ok(result);
        }

        [HttpGet("{reworkRequestId:guid}")]
        [Authorize(Roles = "Lead,QC,QCK,Staff")]
        public async Task<ActionResult<ReworkRequestResponseDTO>> GetReworkRequestById(Guid reworkRequestId)
        {
            var result = await Mediator.Send(new GetRequestByIdQuery(reworkRequestId));
            return Ok(result);
        }

        [HttpGet("{assignmentId:guid}/summary")]
        [Authorize(Roles = "Lead,QC,QCK,Staff")]
        public async Task<ActionResult<ReconcilationSummaryDTO>> GetReworkReconciliationSummary(Guid assignmentId)
        {
            var result = await Mediator.Send(new GetReworkReconciliationSummaryQuery(assignmentId));
            return Ok(result);
        }

        [HttpGet("by-assignId")]
        [Authorize(Roles = "Lead,QC,QCK,Staff")]
        public async Task<ActionResult<ReworkRequestResponseDTO>> GetByAssignId([FromQuery] GetReworkByAssignIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{assignId:guid}/for-dashboard")]
        [Authorize(Roles = "Lead,QC,QCK,Staff")]
        public async Task<ActionResult<ReworkRequestDTO>> GetReworkForDashboard(Guid assignId)
        {
            var result = await Mediator.Send(new GetReworkForDashboardQuery(assignId));
            return Ok(result);
        }

        [HttpGet("by-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<ReworkRequestDTO>>> GetReworkByQcId()
        {
            var result = await Mediator.Send(new GetReworkByQcIdQuery
            {
                QcId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateReworkRequest([FromBody] CreateReworkRequestCommand command)
        {
            command.QCId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Create rework request successfully");
        }

        [HttpPut("{requestId:guid}/rejected")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> RejectReworkRequest(Guid requestId)
        {
            var result = await Mediator.Send(new RejectRequestReworkCommand(requestId));
            return HandleResult(result, "Reject rework request successfully");
        }

        [HttpPut("{requestId:guid}/approved")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> ApproveReworkRequest(Guid requestId, [FromBody] ApproveReworkRequestCommand command)
        {
            command.RequestId = requestId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Approve rework request successfully");
        }
    }
}
