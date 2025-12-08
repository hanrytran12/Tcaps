using System.Security.Claims;
using Application.Common;
using Application.DTOs.Response;
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
    public class MaterialSupplyController : BaseApiController
    {
        [HttpGet()]
        public async Task<Result<List<MaterialSupplyDTO>>> GetAllAsync([FromQuery] string? status)
        {
            var query = new GetAllMaterialSuppliesQuery
            {
                UserId = CurrentUserId,
                Role = User.FindFirstValue(ClaimTypes.Role),
                Status = status
            };

            return await Mediator.Send(query);
        }

        [HttpPost]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] AddMaterialSupplyCommand command)
        {
            command.LeadId = CurrentUserId;
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("qcTransport/InProgress/{supplyId}")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> UpdateInProgressAsync(Guid supplyId)
        {
            var command = new UpdateInProgressByQcTransportCommand
            {
                QcTransportId = CurrentUserId,
                SupplyId = supplyId
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("qc/Completed/{supplyId}")]
        [Authorize(Roles = "QC")]
        public async Task<IActionResult> UpdateCompletedAsync(Guid supplyId, int quantityReceive)
        {
            var command = new CompletedMaterialSupplyCommand
            {
                QcId = CurrentUserId,
                SupplyId = supplyId,
                QuantityReceive = quantityReceive
            };
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("admin/Approve/{supplyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveByAdminAsync(Guid supplyId)
        {
            var command = new UpdateApproveByAdminCommand
            {
                MaterialSupplyId = supplyId
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
