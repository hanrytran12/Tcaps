using Application.DTOs.Response;
using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest;
using Application.Features.TaskTransferRequests.Queries.GetByMaterialRequestIdOrAssignTransferId;
using Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTransferRequestController : BaseApiController
    {
        [HttpGet("all")]
        public async Task<List<TaskTransferRequestDTO>> GetAllAsync([FromQuery] GetAllTaskTransferRequestQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("materialRequestId-assignmentTransferId")]
        public async Task<TaskTransferRequestDTO> GetById([FromQuery] GetByMaterialRequestIdOrAssignTransferIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("for-QcTransport")]
        [Authorize(Roles = "QCTransportOnly")]
        public async Task<List<TaskTransferRequestDTO>> GetByQcTransportAsync([FromQuery] string? status)
        {
            return await Mediator.Send(new GetTaskTransferRequestByQCTransportIdQuery(CurrentUserId, status));
        }

        [HttpPost("for-lead")]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTaskTransferRequestCommand command)
        {
            await Mediator.Send(command);
            return Ok("Tạo yêu cầu chuyển nhiệm vụ thành công");
        }

        [HttpPut("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequestAsync([FromQuery] UpdateApproveTaskTransferRequestCommand command)
        {
            await Mediator.Send(command);
            return Ok("Chấp nhận thành công");
        }
    }
}
