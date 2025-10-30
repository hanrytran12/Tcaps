using Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentDefectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComponentDefectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("resolve/{componentId}")]
        public async Task<IActionResult> UpdateResolveAsync(Guid componentId, [FromQuery] UpdateComponentDefectResolvedCommand command)
        {
            command.Id = componentId;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpPut("confirm/{componentId}")]
        public async Task<IActionResult> UpdateConfirmAsync(Guid componentId, [FromQuery] UpdateComponentDefectConfirmCommand command)
        {
            command.ComponentId = componentId;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }
    }
}
