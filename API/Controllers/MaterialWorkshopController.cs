using Application.DTOs.Response;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId;
using Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWorkshopController : BaseApiController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMaterialWorkshopQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<MaterialWorkshopDTO>> GetByQCIdAsync([FromQuery] Guid workshopId)
        {
            var query = new GetMaterialWorkshopByQCIdQuery
            {
                QC_Id = CurrentUserId,
                WorkshopId = workshopId
            };

            return await Mediator.Send(query);
        }

        [HttpGet("total-quantity-receive")]
        [Authorize(Roles = "QC,Lead")]
        public async Task<int> GetTotalQuantityReceive([FromQuery] Guid batchId)
        {
            var query = new TotalQuantityReceiveQuery
            {
                BatchId = batchId,
                QcId = CurrentUserId
            };
            return await Mediator.Send(query);
        }

        [HttpPut("update-confirm")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateConfirmAsync([FromQuery] UpdateConfirmMaterialWorkshopCommand query)
        {
            await Mediator.Send(query);
            return Ok("QC đã chấp nhận đơn hàng thành công");
        }
    }
}
