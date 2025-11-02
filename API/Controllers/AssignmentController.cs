using Application.DTOs.Request;
using Application.Features.Assignments.Commands.CompleteAssignment;
using Application.Features.Assignments.Commands.PlanAssignments;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{assignmentId:guid}/completion-stats")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CompletionStats(Guid assignmentId)
        {
            var totalQuantity = await _assignmentCompletionService.CalculateCompetedQuantityAsync(assignmentId);
            return Ok(totalQuantity);
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
    }
}
