using Application.DTOs.Response;
using Application.Features.ComponentDefects.Query.GetComponentDefects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QCController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QCController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("rework-requests")]
        public async Task<List<ComponentDefectsDTO>> GetAllComponentDefect([FromQuery] GetComponentDefectsQuery query)
        {
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
