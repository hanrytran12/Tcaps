using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialUseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MaterialUseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterialUse([FromBody] Application.Features.MaterialUse.Commands.AddMaterialUse.AddMaterialUseCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
