using Application.Features.MaterialRequest.Commands.AddMaterialRequest;
using Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialRequestController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MaterialRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterialRequest([FromBody] AddMaterialRequestCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpPut("approve/{id:guid}")]
        public async Task<IActionResult> ApproveMaterialRequest([FromRoute] Guid id)
        {
            var command = new Application.Features.MaterialRequest.Commands.ApproveRequestFromLead.ApproveRequestFromLeadCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }

        [HttpPut("confirmed/{id:guid}")]
        public async Task<IActionResult> ConfirmMaterialRequest([FromRoute] Guid id)
        {
            var command = new ConfirmRequestFromQcCommand { Id = id };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.error);
        }
    }
}
