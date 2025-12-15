using Application.DTOs.Response;
using Application.Features.Inventories.Commands.AddInventory;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("from-{materialId:guid}")]
        public async Task<InventoryHistoryDTO> GetInventoryByMaterialId(Guid materialId, [FromQuery] int month, [FromQuery] int year)
        {
            return await _mediator.Send(new Application.Features.Inventories.Queries.GetInventoryByMaterialId.GetInventoryByMaterialIdQuery(materialId, month, year));
        }


        [HttpGet("{id:guid}")]
        public async Task<Inventory> GetInventoryById(Guid id)
        {
            return await _mediator.Send(new Application.Features.Inventories.Queries.GetInventoryById.GetInventoryByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory([FromForm] AddInventoryCommand command)
        {
            await _mediator.Send(command);
            return Ok("Inventory added successfully");
        }
    }
}
