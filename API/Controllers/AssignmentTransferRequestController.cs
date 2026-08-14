using Application.DTOs.Response;
using Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest;
using Application.Features.AssingmentTransferRequest.Commands.QcTransportReception;
using Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAllForQcTransport;
using Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport;
using Application.Features.AssingmentTransferRequest.Queries.GetReconciliationSummary;
using Application.Features.AssingmentTransferRequest.Queries.GetTransferRequestByAssignmentId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentTransferRequestController : BaseApiController
    {
        public AssignmentTransferRequestController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Lead")]
        public async Task<ActionResult<List<AssignmentTransferRequestDTO>>> GetAllTransferRequest()
        {
            var result = await Mediator.Send(new GetAllTransferRequestQuery(CurrentUserId));
            return Ok(result);
        }

        [HttpGet("{assignmentId:guid}/reconcilliation-summary")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<ReconcilationSummaryDTO>> GetReconciliationSummary(Guid assignmentId)
        {
            var result = await Mediator.Send(new GetReconciliationSummaryQuery(assignmentId));
            return Ok(result);
        }

        [HttpGet("by-assignment/{assignmentId:guid}")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<TransferRequestDTO>> GetTransferRequestByAssignmentId(Guid assignmentId)
        {
            var result = await Mediator.Send(new GetTransferRequestByAssignmentIdQuery(assignmentId));
            return Ok(result);
        }

        [HttpGet("qc-transport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<ActionResult<AssignmentTransferRequestDTO>> GetForQCTransportAsync(
        [FromQuery] GetAssignmentTransferForQcTransportQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("getAll-for-qcTransport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<ActionResult<List<AssignmentTransferRequestDTO>>> GetAllForQcTransport()
        {
            var result = await Mediator.Send(new GetAllForQcTransportQuery
            {
                QcTransportId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "QC,QCK")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] AddAssignmentTransferRequestCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("approved/{transferRequestId:guid}")]
        [Authorize(Policy = "LeadOrValidQCTransport")]
        public async Task<IActionResult> ApproveTransferRequest(Guid transferRequestId, [FromBody] UpdateAssignmentTransferRequestCommand command)
        {
            command = new UpdateAssignmentTransferRequestCommand(transferRequestId, CurrentUserId, command.CompleteQuantityReceive, command.NoteLead);
            await Mediator.Send(command);
            return Ok(new { message = "Yêu cầu chuyển giao đã được phê duyệt thành công." });
        }

        [HttpPut("qc-transport-reception")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> QCTransportReception([FromQuery] Guid assignmentTransferId)
        {
            await Mediator.Send(new QcTransportReceptionCommand
            {
                QCTransportId = CurrentUserId,
                AssignmentTransferRequestId = assignmentTransferId
            });
            return Ok(new { message = "Tiếp nhận yêu cầu chuyển giao thành công." });
        }
    }
}
