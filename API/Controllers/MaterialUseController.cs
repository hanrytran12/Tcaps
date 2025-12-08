using Application.DTOs.Response;
using Application.Features.MaterialUses.Query.GetMaterialUseByAssignId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialUseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("qc/materials/request")]
        public async Task<List<MaterialUseDTO>> GetByAssignIdAsync([FromQuery] GetMaterialUseByAssignIdQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
