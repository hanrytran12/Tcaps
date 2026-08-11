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
        public async Task<List<ReworkRequestDTO>> GetAllReworkRequest()
        {
            return await Mediator.Send(new GetAllReworkRequestQuery());
        }

        [HttpGet("{reworkRequestId:guid}")]
        [Authorize(Roles = "Lead,QC,Staff")]
        public async Task<Domain.Entities.ReworkRequest> GetReworkRequestById(Guid reworkRequestId)
        {
            return await Mediator.Send(new GetRequestByIdQuery(reworkRequestId));
        }

        [HttpGet("{assignmentId:guid}/summary")]
        [Authorize(Roles = "Lead,QC,Staff")]
        public async Task<ReconcilationSummaryDTO> GetReworkReconciliationSummary(Guid assignmentId)
        {
            return await Mediator.Send(new GetReworkReconciliationSummaryQuery(assignmentId));
        }

        [HttpGet("by-assignId")]
        [Authorize(Roles = "Lead,QC,Staff")]
        public async Task<Domain.Entities.ReworkRequest> GetByAssignId([FromQuery] GetReworkByAssignIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{assignId:guid}/for-dashboard")]
        [Authorize(Roles = "Lead,QC,Staff")]
        public async Task<ReworkRequestDTO> GetReworkForDashboard(Guid assignId)
        {
            return await Mediator.Send(new GetReworkForDashboardQuery(assignId));
        }

        [HttpGet("by-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<ReworkRequestDTO>> GetReworkByQcId()
        {
            return await Mediator.Send(new GetReworkByQcIdQuery
            {
                QcId = CurrentUserId
            });
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateReworkRequest([FromBody] CreateReworkRequestCommand command)
        {
            command.QCId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Create rework request successfully");
        }

        [HttpPut("{requestId:guid}/rejected")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> RejectReworkRequest(Guid requestId)
        {
            await Mediator.Send(new RejectRequestReworkCommand(requestId));
            return Ok("Reject rework request successfully");
        }

        [HttpPut("{requestId:guid}/approved")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> ApproveReworkRequest(Guid requestId, [FromBody] ApproveReworkRequestCommand command)
        {
            command.RequestId = requestId;
            await Mediator.Send(command);
            return Ok("Approve rework request successfully");
        }
    }
}
