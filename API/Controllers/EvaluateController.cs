using Application.DTOs.Response;
using Application.Features.Evaluates.Commands.AddEvaluate;
using Application.Features.Evaluates.Queries.GetAllEvaluate;
using Application.Features.Evaluates.Queries.GetEvaluatesByQCId;
using Application.Features.Evaluates.Queries.GetEvaluatesByStaffId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluateController : BaseApiController
    {
        public EvaluateController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,Staff")]
        public async Task<ActionResult<List<EvaluateDTO>>> GetAll()
        {
            var result = await Mediator.Send(new GetAllEvaluateQuery());
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<ActionResult<List<EvaluateDTO>>> GetByQCId([FromQuery] string? status)
        {
            var result = await Mediator.Send(new GetEvaluatesByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            });
            return Ok(result);
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<EvaluateDTO>>> GetByStaffId([FromQuery] Guid assignId)
        {
            var result = await Mediator.Send(new GetEvaluatesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                AssignId = assignId
            });
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateEvaluate([FromForm] AddEvaluateCommand command)
        {
            command.UserId = CurrentUserId;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Đánh giá đã được tạo thành công.");
        }
    }
}
