using Application.Features.MaterialUses.Query.GetMaterialUseByAssignId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialUseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("qc/materials/request")]
        public async Task<IActionResult> GetByAssignIdAsync([FromQuery] GetMaterialUseByAssignIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
