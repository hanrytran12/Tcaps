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
        public async Task<List<EvaluateDTO>> GetAll()
        {
            return await Mediator.Send(new GetAllEvaluateQuery());
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<EvaluateDTO>> GetByQCId([FromQuery] string? status)
        {
            return await Mediator.Send(new GetEvaluatesByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            });
        }

        [HttpGet("for-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<List<EvaluateDTO>> GetByStaffId([FromQuery] Guid assignId)
        {
            return await Mediator.Send(new GetEvaluatesByStaffIdQuery
            {
                StaffId = CurrentUserId,
                AssignId = assignId
            });
        }

        [HttpPost]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> CreateEvaluate([FromForm] AddEvaluateCommand command)
        {
            command.UserId = CurrentUserId;
            await Mediator.Send(command);
            return Ok("Evaluate created successfully");
        }
    }
}
