using Application.Features.ReworkRequest.Commands.CreateReworkRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReworkRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReworkRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReworkRequest([FromBody] CreateReworkRequestCommand command)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            command.QCId = userId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
