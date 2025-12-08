using Application.Common;
using Application.DTOs.Response;
using Application.Features.MaterialSupplies.Command.AddMaterialSupply;
using Application.Features.MaterialSupplies.Command.CompletedMaterialSupply;
using Application.Features.MaterialSupplies.Command.UpdateApproveByAdmin;
using Application.Features.MaterialSupplies.Command.UpdateInProgressByQcTransport;
using Application.Features.MaterialSupplies.Query.GetAllMaterialSupplies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialSupplyController : BaseApiController
    {
        [HttpGet()]
        public async Task<Result<List<MaterialSupplyDTO>>> GetAllAsync([FromQuery] string? status)
        {
            return await Mediator.Send(new GetAllMaterialSuppliesQuery
            {
                UserId = CurrentUserId,
                Role = User.FindFirstValue(ClaimTypes.Role),
                Status = status
            });
        }

        [HttpPost]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] AddMaterialSupplyCommand command)
        {
            command.LeadId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Tạo phiếu cung cấp vật tư thành công");
        }

        [HttpPut("qcTransport/InProgress/{supplyId}")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> UpdateInProgressAsync(Guid supplyId)
        {
            await Mediator.Send(new UpdateInProgressByQcTransportCommand
            {
                QcTransportId = CurrentUserId,
                SupplyId = supplyId
            });
            return Ok("Cập nhật trạng thái thành công");
        }

        [HttpPut("qc/Completed/{supplyId}")]
        [Authorize(Roles = "QC")]
        public async Task<IActionResult> UpdateCompletedAsync(Guid supplyId, int quantityReceive)
        {
            await Mediator.Send(new CompletedMaterialSupplyCommand
            {
                QcId = CurrentUserId,
                SupplyId = supplyId,
                QuantityReceive = quantityReceive
            });
            return Ok("Cập nhật trạng thái thành công");
        }

        [HttpPut("admin/Approve/{supplyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveByAdminAsync(Guid supplyId)
        {
            await Mediator.Send(new UpdateApproveByAdminCommand
            {
                MaterialSupplyId = supplyId
            });
            return Ok("Cập nhật trạng thái thành công");
        }
    }
}
