using Application.DTOs.Response;
using Application.Features.Inventories.Commands.AddInventory;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ISender _sender;
        public InventoryController(ISender sender)
        {
            _sender = sender;

        }

        [HttpGet("from-{materialId:guid}")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<InventoryHistoryDTO> GetInventoryByMaterialId(Guid materialId, [FromQuery] int month, [FromQuery] int year)
        {
            return await _sender.Send(new Application.Features.Inventories.Queries.GetInventoryByMaterialId.GetInventoryByMaterialIdQuery(materialId, month, year));
        }


        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<Inventory> GetInventoryById(Guid id)
        {
            return await _sender.Send(new Application.Features.Inventories.Queries.GetInventoryById.GetInventoryByIdQuery(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> AddInventory([FromForm] AddInventoryCommand command)
        {
            await _sender.Send(command);
            return Ok("Inventory added successfully");
        }
    }
}
