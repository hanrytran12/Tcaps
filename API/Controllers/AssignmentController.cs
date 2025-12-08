using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Features.Assignments.Queries.GetAllAsignmentByQCId;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Features.Assignments.Queries.GetAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetAssignmentsByStaffId;
using Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId;
using Application.Features.Assignments.Queries.NewFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : BaseApiController
    {
        [HttpGet("{assignmentId:guid}/allocated-materials")]
        public async Task<List<AllocatedMaterialDto>> GetAllocatedMaterials(Guid assignmentId)
        {
            return await Mediator.Send(new GetAllocatedMaterialsQuery(assignmentId));
        }

        [HttpGet("for-staff")]
        public async Task<List<AssignForStaffDTO>> GetAssignmentsForStaffById()
        {
            return await Mediator.Send(new GetAssignmentsByStaffIdQuery
            {
                StaffId = CurrentUserId
            });
        }

        [HttpGet("qc/assignments")]
        public async Task<List<AssignForStaffDTO>> GetAssignmentForQCIdAsync()
        {
            return await Mediator.Send(new GetAllAssignmentByQCIdQuery
            {
                QcId = CurrentUserId
            });
        }

        [HttpGet("staff/{batchId}")]
        public async Task<AssignForStaffDTO> GetAssignmentByBatchIdAsync(Guid batchId)
        {
            return await Mediator.Send(new GetAssignmentByBatchIdQuery
            {
                StaffId = CurrentUserId,
                BatchId = batchId
            });
        }

        [HttpGet("qc-lead-admin/assign-history/{batchId}")]
        [Authorize(Roles = "QC,Admin,Lead")]
        public async Task<List<AssignmentHistoryDTO>> GetAssignmentHistoryForQCAsync(Guid batchId)
        {
            return await Mediator.Send(new GetAssignmentForHistoryByBatchIdQuery
            {
                BatchId = batchId
            });
        }

        [HttpGet("qc/detail-assignment/{batchId}")]
        public async Task<List<DashboardAssignmentDTO>> GetDetailAssignmentForQCAsync(Guid batchId)
        {
            return await Mediator.Send(new GetDetailAssignmentByBatchIdQuery
            {
                BatchId = batchId
            });
        }

        [HttpPost("{batchId:guid}/plan-assignments")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> PlanAssignments(Guid batchId, [FromBody] List<AssignmentPlanItemDTO> planItems)
        {
            await Mediator.Send(new PlanAssignmentsCommand
            {
                BatchId = batchId,
                PlanItems = planItems,
            });
            return Ok("Kế hoạch sản xuất đã được tạo thành công.");
        }
    }
}
