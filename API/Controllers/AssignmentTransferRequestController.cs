using Application.Common;
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
            var query = new GetAllTransferRequestQuery();
            return await Mediator.Send(query);
        }

        [HttpGet("{assignmentId:guid}/reconcilliation-summary")]
        public async Task<ReconcilationSummaryDTO> GetReconciliationSummary(Guid assignmentId)
        {
            var query = new GetReconciliationSummaryQuery(assignmentId);
            return await Mediator.Send(query);
        }

        [HttpGet("by-assignment/{assignmentId:guid}")]
        public async Task<TransferRequestDTO> GetTransferRequestByAssignmentId(Guid assignmentId)
        {
            var query = new GetTransferRequestByAssignmentIdQuery(assignmentId);
            return await Mediator.Send(query);
        }

        [HttpGet("qc-transport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<Result<AssignmentTransferRequestDTO>> GetForQCTransportAsync(
        [FromQuery] GetAssignmentTransferForQcTransportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("getAll-for-qcTransport")]
        public async Task<Result<List<AssignmentTransferRequestDTO>>> GetAllForQcTransport()
        {
            var query = new GetAllForQcTransportQuery
            {
                QcTransportId = CurrentUserId
            };

            return await Mediator.Send(query);
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
            var command = new UpdateAssignmentTransferRequestCommand(transferRequestId, CurrentUserId);
            await Mediator.Send(command);
            return Ok("Yêu cầu chuyển giao đã được phê duyệt thành công.");
        }

        [HttpPut("qc-transport-reception")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> QCTransportReception([FromQuery] Guid assignmentTransferId)
        {
            var command = new QcTransportReceptionCommand
            {
                QCTransportId = CurrentUserId,
                AssignmentTransferRequestId = assignmentTransferId
            };

            await Mediator.Send(command);
            return Ok("Tiếp nhận yêu cầu chuyển giao thành công.");
        }
    }
}
