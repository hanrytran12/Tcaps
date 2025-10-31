using Application.Features.ComponentDefects.Query.GetComponentDefects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QCController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QCController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("rework-requests")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetAllComponentDefect([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var userId = Guid.Parse(userIdString);
            var query = new GetComponentDefectsQuery();
            query.QCId = userId;
            query.Status = status;
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
