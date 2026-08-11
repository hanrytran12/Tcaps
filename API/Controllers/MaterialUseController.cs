using Application.DTOs.Response;
using Application.Features.MaterialUses.Query.GetMaterialUseByAssignId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialUseController : ControllerBase
    {
        private readonly ISender _sender;

        public MaterialUseController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("qc/materials/request")]
        [Authorize(Policy = "QC")]
        public async Task<List<MaterialUseDTO>> GetByAssignIdAsync([FromQuery] GetMaterialUseByAssignIdQuery query)
        {
            return await _sender.Send(query);
        }
    }
}
