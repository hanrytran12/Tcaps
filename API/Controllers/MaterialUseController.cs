using Application.DTOs.Response;
using Application.Features.MaterialUses.Query.GetMaterialUseByAssignId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseController : BaseApiController
    {
        public MaterialUseController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet("qc/materials/request")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<MaterialUseDTO>>> GetByAssignIdAsync([FromQuery] GetMaterialUseByAssignIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
