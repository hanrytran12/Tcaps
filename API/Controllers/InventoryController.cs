using Application.DTOs.Response;
using Application.Features.Inventories.Commands.AddInventory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : BaseApiController
    {
        public InventoryController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("from-{materialId:guid}")]
        [Authorize(Roles = "Admin,Lead,QC,QCK")]
        public async Task<ActionResult<InventoryHistoryDTO>> GetInventoryByMaterialId(Guid materialId, [FromQuery] int month, [FromQuery] int year)
        {
            var result = await Mediator.Send(new Application.Features.Inventories.Queries.GetInventoryByMaterialId.GetInventoryByMaterialIdQuery(materialId, month, year));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<ActionResult<InventoryResponseDTO>> GetInventoryById(Guid id)
        {
            var result = await Mediator.Send(new Application.Features.Inventories.Queries.GetInventoryById.GetInventoryByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> AddInventory([FromForm] AddInventoryCommand command)
        {
            await Mediator.Send(command);
            return Ok(new { message = "Tạo kho thành công." });
        }
    }
}
