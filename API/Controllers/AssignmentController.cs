using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Features.Assignments.Commands.UpdateReadyForTransfer;
using Application.Features.Assignments.Queries.GetAllAsignmentByQCId;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Features.Assignments.Queries.GetAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetAssignmentsByStaffId;
using Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetTaskProgressByQCId;
using Application.Features.Assignments.Queries.NewFolder;
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
        public async Task<List<AllocatedMaterialDto>> GetAllocatedMaterials(Guid assignmentId)
        {
            return await Mediator.Send(new GetAllocatedMaterialsQuery(assignmentId));
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<List<AssignForStaffDTO>> GetAssignmentsForStaffById()
        {
            return await Mediator.Send(new GetAssignmentsByStaffIdQuery
            {
                StaffId = CurrentUserId
            });
        }

        [HttpGet("qc/assignments")]
        [Authorize(Policy = "QC")]
        public async Task<List<AssignForStaffDTO>> GetAssignmentForQCIdAsync()
        {
            return await Mediator.Send(new GetAllAssignmentByQCIdQuery
            {
                QcId = CurrentUserId
            });
        }

        [HttpGet("staff/{batchId}")]
        [Authorize(Roles = "Staff")]
        public async Task<AssignForStaffDTO> GetAssignmentByBatchIdAsync(Guid batchId)
        {
            return await Mediator.Send(new GetAssignmentByBatchIdQuery
            {
                StaffId = CurrentUserId,
                BatchId = batchId
            });
        }

        [HttpGet("qc-lead-admin/assign-history/{batchId}")]
        [Authorize(Roles = "QC,QCK,Admin,Lead")]
        public async Task<List<AssignmentHistoryDTO>> GetAssignmentHistoryForQCAsync(Guid batchId)
        {
            return await Mediator.Send(new GetAssignmentForHistoryByBatchIdQuery
            {
                BatchId = batchId,
                UserId = CurrentUserId
            });
        }

        [HttpGet("qc/detail-assignment/{batchId}")]
        [Authorize(Policy = "QC")]
        public async Task<List<DashboardAssignmentDTO>> GetDetailAssignmentForQCAsync(Guid batchId)
        {
            return await Mediator.Send(new GetDetailAssignmentByBatchIdQuery
            {
                BatchId = batchId
            });
        }

        [HttpGet("qc-staff/task-progress")]
        [Authorize(Policy = "QC")]
        public async Task<List<TaskProgressDTO>> GetTaskProgressByQcIdAsync()
        {
            return await Mediator.Send(new GetTaskProgressByQCIdQuery
            {
                QcId = CurrentUserId,
            });
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
            return Ok("Kế hoạch sản xuất đã được tạo thành công.");
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
            return Ok("Cập nhật trạng thái sẵn sàng chuyển giao thành công.");
        }
    }
}
