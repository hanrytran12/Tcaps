using Application.Features.Materials.Queries;
using Application.Features.Materials.Queries.GetAllMaterialToWatch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMaterialsAsync()
        {
            var query = new GetAllMaterialToWatchQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllMaterialQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
