using Application.Features.ReworkRequest.Commands.ApproveReworkRequest;
using Application.Features.ReworkRequest.Commands.CreateReworkRequest;
using Application.Features.ReworkRequest.Commands.RejectReworkRequest;
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

        [HttpPut("{requestId:guid}/rejected")]
        public async Task<IActionResult> RejectReworkRequest(Guid requestId)
        {
            var query = new RejectRequestReworkCommand(requestId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{requestId:guid}/approved")]
        public async Task<IActionResult> ApproveReworkRequest(Guid requestId, [FromBody] ApproveReworkRequestCommand command)
        {
            command.RequestId = requestId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
