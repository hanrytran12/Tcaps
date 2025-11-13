using Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory;
using Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopInventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkshopInventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkshopInventory()
        {
            var query = new GetAllWorkshopInventoryQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{workshopId:guid}")]
        public async Task<IActionResult> GetWorkshopInvenntoryByWorkshopId(Guid workshopId)
        {
            var query = new GetWorkshopInventoryByWorkshopIdQuery(workshopId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
