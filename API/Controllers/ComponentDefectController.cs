using Application.DTOs.Response;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.ComponentDefect.Query.GetComponentByEvaluateId;
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

        [HttpGet("for-staff")]
        public async Task<List<ComponentDefectsDTO>> GetAllByEvaluateIdForStaffAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("for-qc")]
        public async Task<List<ComponentDefectsDTO>> GetAllByEvaluateIdForQCAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPut("resolve/{componentId}")]
        public async Task<IActionResult> UpdateResolveAsync(Guid componentId, [FromQuery] UpdateComponentDefectResolvedCommand command)
        {
            command.Id = componentId;
            await _mediator.Send(command);
            return Ok("Sửa chữa thành công.");
        }

        [HttpPut("confirm/{componentId}")]
        public async Task<IActionResult> UpdateConfirmAsync(Guid componentId, [FromQuery] UpdateComponentDefectConfirmCommand command)
        {
            command.ComponentId = componentId;
            await _mediator.Send(command);
            return Ok("Chấp nhận đã sửa thành công.");
        }
    }
}
