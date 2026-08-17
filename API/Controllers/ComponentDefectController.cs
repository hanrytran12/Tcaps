using Application.DTOs.Response;
using Application.Features.ComponentDefect.Commands.RejectComponentFromQC;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm;
using Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve;
using Application.Features.ComponentDefect.Query.GetComponentByEvaluateId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentDefectController : BaseApiController
    {
        public ComponentDefectController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<ComponentDefectsDTO>>> GetAllByEvaluateIdForStaffAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<ComponentDefectsDTO>>> GetAllByEvaluateIdForQCAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("resolve/{componentId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateResolveAsync(Guid componentId, [FromBody] UpdateComponentDefectResolvedCommand command)
        {
            command.Id = componentId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Sửa chữa thành công.");
        }

        [HttpPut("confirm/{componentId}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateConfirmAsync(Guid componentId, [FromBody] UpdateComponentDefectConfirmCommand command)
        {
            command.ComponentId = componentId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Chấp nhận đã sửa thành công.");
        }

        [HttpPut("reject/{componentId}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> RejectComponentAsync(Guid componentId, [FromBody] RejectComponentFromQCCommand command)
        {
            command.Id = componentId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Từ chối từ QC.");
        }
    }
}
