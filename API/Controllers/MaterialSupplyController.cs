using System.Security.Claims;
using Application.Features.MaterialSupplies.Command.AddMaterialSupply;
using Application.Features.MaterialSupplies.Command.CompletedMaterialSupply;
using Application.Features.MaterialSupplies.Command.UpdateApproveByAdmin;
using Application.Features.MaterialSupplies.Command.UpdateInProgressByQcTransport;
using Application.Features.MaterialSupplies.Query.GetAllMaterialSupplies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialSupplyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialSupplyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAllMaterialSuppliesQuery
            {
                UserId = Guid.Parse(userIdString),
                Role = role,
                Status = status
            };

            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] AddMaterialSupplyCommand command)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }
            command.LeadId = Guid.Parse(userIdString);
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.IsFailure);

            return Ok(result);
        }

        [HttpPut("qcTransport/InProgress/{supplyId}")]
        [Authorize(Roles = "QCTransport")]
        public async Task<IActionResult> UpdateInProgressAsync(Guid supplyId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var command = new UpdateInProgressByQcTransportCommand
            {
                QcTransportId = Guid.Parse(userIdString),
                SupplyId = supplyId
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.IsFailure);

            return Ok(result);
        }

        [HttpPut("qc/Completed/{supplyId}")]
        [Authorize(Roles = "QC")]
        public async Task<IActionResult> UpdateCompletedAsync(Guid supplyId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var command = new CompletedMaterialSupplyCommand
            {
                QcId = Guid.Parse(userIdString),
                SupplyId = supplyId
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.IsFailure);

            return Ok(result);
        }

        [HttpPut("admin/Approve/{supplyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveByAdminAsync(Guid supplyId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var command = new UpdateApproveByAdminCommand
            {
                MaterialSupplyId = supplyId
            };
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result.IsFailure);

            return Ok(result);
        }
    }
}
