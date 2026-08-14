using Application.DTOs.Response;
using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetByMaterialRequestIdOrAssignTransferId;
using Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTransferRequestController : BaseApiController
    {
        public TaskTransferRequestController(ISender mediator) : base(mediator)
        {
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<ActionResult<List<TaskTransferRequestDTO>>> GetAllAsync([FromQuery] GetAllTaskTransferRequestQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("materialRequestId-assignmentTransferId")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport")]
        public async Task<ActionResult<TaskTransferRequestDTO>> GetById([FromQuery] GetByMaterialRequestIdOrAssignTransferIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-QcTransport")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<ActionResult<List<TaskTransferRequestDTO>>> GetByQcTransportAsync([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetTaskTransferRequestByQCTransportIdQuery(CurrentUserId, status));
            return Ok(result);
        }

        [HttpPost("for-lead")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskTransferRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Tạo yêu cầu chuyển nhiệm vụ thành công");
        }

        [HttpPut("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequestAsync([FromBody] UpdateApproveTaskTransferRequestCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Chấp nhận thành công");
        }
    }
}
