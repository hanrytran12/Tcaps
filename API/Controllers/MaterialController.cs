using Application.DTOs.Response;
using Application.Features.Materials.Commands.AddMaterial;
using Application.Features.Materials.Queries;
using Application.Features.Materials.Queries.GetAllMaterialToWatch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly ISender _sender;

        public MaterialController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,Staff")]
        public async Task<List<MaterialToWatchDTO>> GetAllMaterialsAsync()
        {
            return await _sender.Send(new GetAllMaterialToWatchQuery());
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin,Lead,QC,Staff")]
        public async Task<List<MaterialDTO>> GetAllAsync([FromQuery] GetAllMaterialQuery query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> CreateMaterial([FromBody] AddMaterialCommand command)
        {
            await _sender.Send(command);
            return Ok("Material created successfully");
        }
    }
}
