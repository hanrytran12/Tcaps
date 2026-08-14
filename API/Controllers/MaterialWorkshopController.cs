using Application.DTOs.Response;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId;
using Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWorkshopController : BaseApiController
    {
        public MaterialWorkshopController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Lead,QC,QCK")]
        public async Task<ActionResult<List<MaterialWorkshopSummaryDTO>>> GetAllAsync([FromQuery] GetAllMaterialWorkshopQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<MaterialWorkshopDTO>>> GetByQCIdAsync([FromQuery] Guid workshopId)
        {
            var result = await Mediator.Send(new GetMaterialWorkshopByQCIdQuery { QC_Id = CurrentUserId, WorkshopId = workshopId });
            return Ok(result);
        }

        [HttpGet("total-quantity-receive")]
        [Authorize(Roles = "QC,QCK,Lead")]
        public async Task<ActionResult<int>> GetTotalQuantityReceive([FromQuery] Guid batchId)
        {
            var result = await Mediator.Send(new TotalQuantityReceiveQuery { BatchId = batchId, QcId = CurrentUserId });
            return Ok(result);
        }

        [HttpPut("update-confirm")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> UpdateConfirmAsync([FromQuery] UpdateConfirmMaterialWorkshopCommand query)
        {
            var result = await Mediator.Send(query);
            return HandleResult(result, "QC đã chấp nhận đơn hàng thành công");
        }
    }
}
