using Application.Features.Assignments.Commands.AddAssignmentCommand;
using Application.Features.Assignments.Commands.CompleteAssignment;
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

        [HttpPost]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> AddAssignment([FromBody] AddAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
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
