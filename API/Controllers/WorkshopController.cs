using Application.Features.Workshop.Queries.GetWorkshopTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkshopController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WorkshopController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkshopsTemplate()
        {
            var result = await _mediator.Send(new GetWorkshopTemplateQuery());
            return Ok(result);
        }
    }
}
