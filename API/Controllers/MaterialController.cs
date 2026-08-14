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
    public class MaterialController : BaseApiController
    {
        public MaterialController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<List<MaterialToWatchDTO>>> GetAllMaterialsAsync()
        {
            var result = await Mediator.Send(new GetAllMaterialToWatchQuery());
            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Lead,QC,QCK,QCTransport,Staff")]
        public async Task<ActionResult<List<MaterialDTO>>> GetAllAsync([FromQuery] GetAllMaterialQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lead")]
        public async Task<IActionResult> CreateMaterial([FromBody] AddMaterialCommand command)
        {
            await Mediator.Send(command);
            return Ok(new { message = "Tạo vật liệu thành công." });
        }
    }
}
