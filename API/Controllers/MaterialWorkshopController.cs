using System.Security.Claims;
using Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWorkshopController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialWorkshopController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMaterialWorkshopQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetByQCIdAsync([FromQuery] string? status, [FromQuery] Guid workshopId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetMaterialWorkshopByQCIdQuery
            {
                QC_Id = Guid.Parse(userIdString),
                Status = status,
                WorkshopId = workshopId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("for-lead")]
        [Authorize(Roles = "Lead,QCTransport")]
        public async Task<IActionResult> CreateMaterialWorkshop(AddMaterialWorkshopCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var isQcTransport = User.FindFirstValue("isQcTransport");

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            // Chỉ cho phép nếu có claim isQcTransport = true
            if (role == "QCTransport" && isQcTransport?.ToLower() != "true")
            {
                return Forbid("QCTransport cần có quyền isQcTransport = true để truy cập.");
            }

            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpPut("update-confirm")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateConfirmAsync([FromQuery] UpdateConfirmMaterialWorkshopCommand query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }
    }
}
