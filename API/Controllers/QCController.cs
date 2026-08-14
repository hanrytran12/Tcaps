using Application.DTOs.Response;
using Application.Features.ComponentDefects.Query.GetComponentDefects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QCController : BaseApiController
    {
        public QCController(ISender mediator) : base(mediator)
        {
        }
        [HttpGet("rework-requests")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<ComponentDefectsDTO>>> GetAllComponentDefect([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetComponentDefectsQuery
            {
                QCId = CurrentUserId,
                Status = status
            });
            return Ok(result);
        }
    }
}
