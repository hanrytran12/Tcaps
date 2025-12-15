using Application.DTOs.Response;
using Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest;
using Application.Features.AssingmentTransferRequest.Commands.QcTransportReception;
using Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAllForQcTransport;
using Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest;
using Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport;
using Application.Features.AssingmentTransferRequest.Queries.GetReconciliationSummary;
using Application.Features.AssingmentTransferRequest.Queries.GetTransferRequestByAssignmentId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentTransferRequestController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "Lead")]
        public async Task<ActionResult<List<AssignmentTransferRequestDTO>>> GetAllTrasnferRequest()
        {
            return await Mediator.Send(new GetAllTransferRequestQuery());
        }

        [HttpGet("{assignmentId:guid}/reconcilliation-summary")]
        public async Task<ReconcilationSummaryDTO> GetReconciliationSummary(Guid assignmentId)
        {
            return await Mediator.Send(new GetReconciliationSummaryQuery(assignmentId));
        }

        [HttpGet("by-assignment/{assignmentId:guid}")]
        public async Task<TransferRequestDTO> GetTransferRequestByAssignmentId(Guid assignmentId)
        {
            return await Mediator.Send(new GetTransferRequestByAssignmentIdQuery(assignmentId));
        }

        [HttpGet("qc-transport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<AssignmentTransferRequestDTO> GetForQCTransportAsync(
        [FromQuery] GetAssignmentTransferForQcTransportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("getAll-for-qcTransport")]
        public async Task<List<AssignmentTransferRequestDTO>> GetAllForQcTransport()
        {
            return await Mediator.Send(new GetAllForQcTransportQuery
            {
                QcTransportId = CurrentUserId
            });
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateTransferRequest([FromBody] AddAssignmentTransferRequestCommand command)
        {
            command.UserId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Yêu cầu chuyển giao đã được tạo thành công.");
        }

        [HttpPut("approved/{transferRequestId:guid}")]
        [Authorize(Policy = "LeadOrValidQCTransport")]
        public async Task<IActionResult> ApproveTrasnferRequest(Guid transferRequestId)
        {
            await Mediator.Send(new UpdateAssignmentTransferRequestCommand(transferRequestId, CurrentUserId));
            return Ok("Yêu cầu chuyển giao đã được phê duyệt thành công.");
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
            return Ok("Tiếp nhận yêu cầu chuyển giao thành công.");
        }
    }
}
