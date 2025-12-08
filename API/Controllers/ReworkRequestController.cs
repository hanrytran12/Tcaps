using Application.DTOs.Response;
using Application.Features.ReworkRequest.Commands.ApproveReworkRequest;
using Application.Features.ReworkRequest.Commands.CreateReworkRequest;
using Application.Features.ReworkRequest.Commands.RejectReworkRequest;
using Application.Features.ReworkRequest.Queries.GetAllReworkRequest;
using Application.Features.ReworkRequest.Queries.GetRequestById;
using Application.Features.ReworkRequest.Queries.GetReworkByAssignId;
using Application.Features.ReworkRequest.Queries.GetReworkForDashboard;
using Application.Features.ReworkRequest.Queries.GetReworkReconciliationSummary;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReworkRequestController : BaseApiController
    {
        [HttpGet]
        public async Task<List<ReworkRequestDTO>> GetAllReworkRequest()
        {
            return await Mediator.Send(new GetAllReworkRequestQuery());
        }

        [HttpGet("{reworkRequestId:guid}")]
        public async Task<Domain.Entities.ReworkRequest> GetReworkRequestById(Guid reworkRequestId)
        {
            return await Mediator.Send(new GetRequestByIdQuery(reworkRequestId));
        }

        [HttpGet("{assignmentId:guid}/summary")]
        public async Task<ReconcilationSummaryDTO> GetReworkReconciliationSummary(Guid assignmentId)
        {
            return await Mediator.Send(new GetReworkReconciliationSummaryQuery(assignmentId));
        }

        [HttpGet("by-assignId")]
        public async Task<Domain.Entities.ReworkRequest> GetByAssignId([FromQuery] GetReworkByAssignIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{assignId:guid}/for-dashboard")]
        public async Task<ReworkRequestDTO> GetReworkForDashboard(Guid assignId)
        {
            return await Mediator.Send(new GetReworkForDashboardQuery(assignId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReworkRequest([FromBody] CreateReworkRequestCommand command)
        {
            command.QCId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Create rework request successfully");
        }

        [HttpPut("{requestId:guid}/rejected")]
        public async Task<IActionResult> RejectReworkRequest(Guid requestId)
        {
            await Mediator.Send(new RejectRequestReworkCommand(requestId));
            return Ok("Reject rework request successfully");
        }

        [HttpPut("{requestId:guid}/approved")]
        public async Task<IActionResult> ApproveReworkRequest(Guid requestId, [FromBody] ApproveReworkRequestCommand command)
        {
            command.RequestId = requestId;
            await Mediator.Send(command);
            return Ok("Approve rework request successfully");
        }
    }
}
