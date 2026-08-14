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
using Application.Features.MaterialRequest.Queries.GetForAssignmentDashboard;
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
        public MaterialRequestController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<IActionResult> GetAllRequest()
        {
            var query = new GetAllMaterialRequestQuery();
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("pending-confirmation")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<PendingRequestDTO>>> GetPendingRequests()
        {
            var query = new GetPendingRequestForQcQuery(CurrentUserId);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("lead/admin/all-request")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMaterialRequestForAdminQuery query)
        {
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("qc/request")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetByQCIdAsync([FromQuery] string? status)
        {
            var query = new GetMaterialRequestForQCQuery
            {
                QcId = CurrentUserId,
                Status = status
            };
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("qc-transport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> GetRequestsForQcTransport([FromQuery] GetMaterialRequestForQcTransportQuery query)
        {
            var result = await Mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("assignment-dashboard")]
        [Authorize(Roles = "Lead,QC,QCK,Staff")]
        public async Task<ActionResult<List<MaterialRequestForAssignmentDashboardDTO>>> GetForAssignmentDashboard([FromQuery] GetForAssignmentDashboardQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
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

            var result = await Mediator.Send(command);
            return HandleResult(result, "Cung cấp NVL thành công");
        }

        [HttpPost("qc/material-requests")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateMaterialRequestAsync([FromBody] CreateMaterialRequestFromQCCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Yêu cầu cung cấp thêm NVL thành công");
        }

        [HttpPut("approve/{id:guid}")]
        [Authorize(Policy = ("Lead"))]
        public async Task<IActionResult> ApproveMaterialRequest([FromRoute] Guid id)
        {
            var command = new Application.Features.MaterialRequest.Commands.ApproveRequestFromLead.ApproveRequestFromLeadCommand { Id = id };
            var result = await Mediator.Send(command);
            return HandleResult(result, "Duyệt yêu cầu thành công");
        }

        [HttpPut("confirmed/{id:guid}")]
        [Authorize(Roles = "QC,QCK,Lead")]
        public async Task<IActionResult> ConfirmMaterialRequest([FromRoute] Guid id, [FromBody] ConfirmRequestFromQcCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Xác nhận yêu cầu thành công");
        }

        [HttpPut("rejected/{id:guid}")]
        [Authorize(Policy = ("QC"))]
        public async Task<IActionResult> RejectMaterialRequest([FromRoute] Guid id, [FromBody] RejectMaterialRequestCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Từ chối yêu cầu thành công");
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
            return HandleResult(result, "Tiếp nhận NVL thành công");
        }

        [HttpPut("lead-confirm")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> LeadConfirm([FromQuery] ConfirmRequestFromLeadCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Duyệt yêu cầu cho QC vận chuyển thành công.");
        }
    }
}
