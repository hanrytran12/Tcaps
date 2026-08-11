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
    public class ComponentDefectController : ControllerBase
    {
        private readonly ISender _sender;

        public ComponentDefectController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<List<ComponentDefectsDTO>> GetAllByEvaluateIdForStaffAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<ComponentDefectsDTO>> GetAllByEvaluateIdForQCAsync([FromQuery] GetComponentByEvaluatedIdQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpPut("resolve/{componentId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateResolveAsync(Guid componentId, [FromQuery] UpdateComponentDefectResolvedCommand command)
        {
            command.Id = componentId;
            await _sender.Send(command);
            return Ok("Sửa chữa thành công.");
        }

        [HttpPut("confirm/{componentId}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateConfirmAsync(Guid componentId, [FromQuery] UpdateComponentDefectConfirmCommand command)
        {
            command.ComponentId = componentId;
            await _sender.Send(command);
            return Ok("Chấp nhận đã sửa thành công.");
        }

        [HttpPut("reject/{componentId}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> RejectComponentAsync(Guid componentId, [FromQuery] RejectComponentFromQCCommand command)
        {
            command.Id = componentId;
            await _sender.Send(command);
            return Ok("Từ chối từ QC.");
        }
    }
}
