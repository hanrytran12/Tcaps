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
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialSupplyController : BaseApiController
    {
        public MaterialSupplyController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet()]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetAllMaterialSuppliesQuery
            {
                UserId = CurrentUserId,
                Role = User.FindFirstValue(ClaimTypes.Role),
                Status = status
            });
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Lead")]
        public async Task<IActionResult> CreateAsync([FromBody] AddMaterialSupplyCommand command)
        {
            command.LeadId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Tạo phiếu cung cấp vật tư thành công");
        }

        [HttpPut("qcTransport/InProgress/{supplyId}")]
        [Authorize(Policy = "QCTransportOnly")]
        public async Task<IActionResult> UpdateInProgressAsync(Guid supplyId)
        {
            var result = await Mediator.Send(new UpdateInProgressByQcTransportCommand
            {
                QcTransportId = CurrentUserId,
                SupplyId = supplyId
            });
            return HandleResult(result, "Cập nhật trạng thái thành công");
        }

        [HttpPut("qc/Completed/{supplyId}")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateCompletedAsync(Guid supplyId, int quantityReceive, string? note)
        {
            var result = await Mediator.Send(new CompletedMaterialSupplyCommand
            {
                QcId = CurrentUserId,
                SupplyId = supplyId,
                QuantityReceive = quantityReceive,
                Note = note
            });
            return HandleResult(result, "Cập nhật trạng thái thành công");
        }

        [HttpPut("admin/Approve/{supplyId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveByAdminAsync(Guid supplyId)
        {
            var result = await Mediator.Send(new UpdateApproveByAdminCommand
            {
                MaterialSupplyId = supplyId
            });
            return HandleResult(result, "Cập nhật trạng thái thành công");
        }
    }
}
