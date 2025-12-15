using Application.DTOs.Response;
using Application.Features.ComponentDefects.Query.GetComponentDefects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QCController : BaseApiController
    {
        [HttpGet("rework-requests")]
        [Authorize(Policy = "QC")]
        public async Task<List<ComponentDefectsDTO>> GetAllComponentDefect([FromQuery] string? status)
        {
            return await Mediator.Send(new GetComponentDefectsQuery
            {
                QCId = CurrentUserId,
                Status = status
            });
        }
    }
}
