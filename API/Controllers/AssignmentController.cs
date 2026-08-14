using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Features.Assignments.Commands.UpdateReadyForTransfer;
using Application.Features.Assignments.Queries.GetAllAsignmentByQCId;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Features.Assignments.Queries.GetAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetAssignmentForHistoryByBatchId;
using Application.Features.Assignments.Queries.GetAssignmentsByStaffId;
using Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetTaskProgressByQCId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : BaseApiController
    {
        public AssignmentController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("{assignmentId:guid}/allocated-materials")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,Staff")]
        public async Task<ActionResult<List<AllocatedMaterialDto>>> GetAllocatedMaterials(Guid assignmentId)
        {
            var result = await Mediator.Send(new GetAllocatedMaterialsQuery(assignmentId));
            return Ok(result);
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<AssignForStaffDTO>>> GetAssignmentsForStaffById()
        {
            var result = await Mediator.Send(new GetAssignmentsByStaffIdQuery
            {
                StaffId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpGet("qc/assignments")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<AssignForStaffDTO>>> GetAssignmentForQCIdAsync()
        {
            var result = await Mediator.Send(new GetAllAssignmentByQCIdQuery
            {
                QcId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpGet("staff/{batchId}")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<AssignForStaffDTO>> GetAssignmentByBatchIdAsync(Guid batchId)
        {
            var result = await Mediator.Send(new GetAssignmentByBatchIdQuery
            {
                StaffId = CurrentUserId,
                BatchId = batchId
            });
            return Ok(result);
        }

        [HttpGet("qc-lead-admin/assign-history/{batchId}")]
        [Authorize(Roles = "QC,QCK,Admin,Lead")]
        public async Task<ActionResult<List<AssignmentHistoryDTO>>> GetAssignmentHistoryForQCAsync(Guid batchId)
        {
            var result = await Mediator.Send(new GetAssignmentForHistoryByBatchIdQuery
            {
                BatchId = batchId,
                UserId = CurrentUserId
            });
            return Ok(result);
        }

        [HttpGet("qc/detail-assignment/{batchId}")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<DashboardAssignmentDTO>>> GetDetailAssignmentForQCAsync(Guid batchId)
        {
            var result = await Mediator.Send(new GetDetailAssignmentByBatchIdQuery
            {
                BatchId = batchId
            });
            return Ok(result);
        }

        [HttpGet("qc-staff/task-progress")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<TaskProgressDTO>>> GetTaskProgressByQcIdAsync()
        {
            var result = await Mediator.Send(new GetTaskProgressByQCIdQuery
            {
                QcId = CurrentUserId,
            });
            return Ok(result);
        }

        [HttpPost("{batchId:guid}/plan-assignments")]
        [Authorize(Roles = "Lead,Admin")]
        public async Task<IActionResult> PlanAssignments(Guid batchId, [FromBody] List<AssignmentPlanItemDTO> planItems)
        {
            await Mediator.Send(new PlanAssignmentsCommand
            {
                BatchId = batchId,
                PlanItems = planItems,
            });
            return Ok(new { message = "Kế hoạch sản xuất đã được tạo thành công." });
        }

        [HttpPut("update-ready-for-transfer")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateReadyForTransfer([FromQuery] Guid assignmentId)
        {
            await Mediator.Send(new UpdateReadyForTransferCommand
            {
                AssignmentId = assignmentId,
                QcId = CurrentUserId
            });
            return Ok(new { message = "Cập nhật trạng thái sẵn sàng chuyển giao thành công." });
        }
    }
}
