using Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop;
using Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialWorkshopController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialWorkshopController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("for-lead")]
        public async Task<IActionResult> CreateMaterialWorkshop(AddMaterialWorkshopCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpPut("update-confirm")]
        public async Task<IActionResult> UpdateConfirmAsync([FromQuery] UpdateConfirmMaterialWorkshopCommand query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }
    }
}
