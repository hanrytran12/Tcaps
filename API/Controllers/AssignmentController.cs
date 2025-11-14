using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Assignments.Commands.CompleteAssignment;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Features.Assignments.Queries.GetAllAsignmentByQCId;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Features.Assignments.Queries.GetAssignmentByBatchId;
using Application.Features.Assignments.Queries.GetAssignmentsByStaffId;
using Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId;
using Application.Features.Assignments.Queries.NewFolder;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAssignmentCompletionService _assignmentCompletionService;

        public AssignmentController(IMediator mediator, IAssignmentCompletionService assignmentCompletionService)
        {
            _mediator = mediator;
            _assignmentCompletionService = assignmentCompletionService;
        }

        [HttpGet("{assignmentId:guid}/allocated-materials")]
        public async Task<ActionResult<List<AllocatedMaterialDto>>> GetAllocatedMaterials(Guid assignmentId)
        {
            var query = new GetAllocatedMaterialsQuery(assignmentId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{batchId:guid}/plan-assignments")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> PlanAssignments(Guid batchId, [FromBody] List<AssignmentPlanItemDTO> planItems)
        {
            var command = new PlanAssignmentsCommand
            {
                BatchId = batchId,
                PlanItems = planItems
            };

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok("Kế hoạch sản xuất đã được tạo thành công.");
            }

            return BadRequest(result.error);
        }

        [HttpPut("{id:guid}/complete")]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> CompleteAssignment(Guid id)
        {
            var command = new CompleteAssignmentCommand { AssignmentId = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.error);
        }

        [HttpGet("for-staff")]
        public async Task<IActionResult> GetAssignmentsForStaffById()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAssignmentsByStaffIdQuery
            {
                StaffId = Guid.Parse(userIdString)
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("qc/assignments")]
        public async Task<IActionResult> GetAssignmentForQCIdAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAllAssignmentByQCIdQuery
            {
                QcId = Guid.Parse(userIdString)
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("staff/{batchId}")]
        public async Task<IActionResult> GetAssignmentByBatchIdAsync(Guid batchId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAssignmentByBatchIdQuery
            {
                StaffId = Guid.Parse(userIdString),
                BatchId = batchId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc/assign-history/{batchId}")]
        public async Task<IActionResult> GetAssignmentHistoryForQCAsync(Guid batchId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAssignmentForHistoryByBatchIdQuery
            {
                QcId = Guid.Parse(userIdString),
                BatchId = batchId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("qc/detail-assignment/{batchId}")]
        public async Task<IActionResult> GetDetailAssignmentForQCAsync(Guid batchId)
        {
            var query = new GetDetailAssignmentByBatchIdQuery
            {
                BatchId = batchId
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
