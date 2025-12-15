using Application.DTOs.Response;
using Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<AuthRepsponseDTO> LoginAsync([FromQuery] LoginQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
