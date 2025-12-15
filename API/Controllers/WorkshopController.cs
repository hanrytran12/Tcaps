using Application.DTOs.Response;
using Application.Features.Workshop.Command.AddWorkshop;
using Application.Features.Workshop.Queries.GetWorkshopTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<List<WorkshopsDTO>> GetWorkshopsTemplate()
        {
            return await _mediator.Send(new GetWorkshopTemplateQuery());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddWorkshop([FromBody] AddWorkshopCommand command)
        {
            await _mediator.Send(command);
            return Ok("Workshop added successfully");
        }
    }
}
