using Application.Features.Assignments.Commands.AddAssignmentCommand;
using Application.Features.Assignments.Commands.CompleteAssignment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment([FromBody] AddAssignmentCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("{id:guid}/complete")]
        public async Task<IActionResult> CompleteAssignment(Guid id)
        {
            var command = new CompleteAssignmentCommand { AssignmentId = id };
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.error);
        }
    }
}
