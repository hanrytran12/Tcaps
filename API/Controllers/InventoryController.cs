using Application.Features.Inventories.Commands.AddInventory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInventoryById(Guid id)
        {
            var result = await _mediator.Send(new Application.Features.Inventories.Queries.GetInventoryById.GetInventoryByIdQuery(id));
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(result.Error);
        }

        [HttpPost]
        [Authorize(Policy = "Lead")]
        public async Task<IActionResult> AddInventory([FromForm] AddInventoryCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
