using Application.DTOs.Response;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId;
using Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWorkshopController : BaseApiController
    {
        [HttpGet("all")]
        public async Task<List<MaterialWorkshop>> GetAllAsync([FromQuery] GetAllMaterialWorkshopQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<MaterialWorkshopDTO>> GetByQCIdAsync([FromQuery] Guid workshopId)
        {
            return await Mediator.Send(new GetMaterialWorkshopByQCIdQuery { QC_Id = CurrentUserId, WorkshopId = workshopId });
        }

        [HttpGet("total-quantity-receive")]
        [Authorize(Roles = "QC,Lead")]
        public async Task<int> GetTotalQuantityReceive([FromQuery] Guid batchId)
        {
            return await Mediator.Send(new TotalQuantityReceiveQuery { BatchId = batchId, QcId = CurrentUserId });
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
