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
        private readonly IMediator _mediator;

        public EvaluateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<EvaluateDTO>> GetAll()
        {
            return await _mediator.Send(new GetAllEvaluateQuery());
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<List<EvaluateDTO>> GetByQCId([FromQuery] string? status)
        {
            return await _mediator.Send(new GetEvaluatesByQCIdQuery
            {
                QC_Id = CurrentUserId,
                Status = status
            });
        }

        [HttpGet("for-staff")]
        public async Task<List<EvaluateDTO>> GetByStaffId([FromQuery] Guid assignId)
        {
            return await _mediator.Send(new GetEvaluatesByStaffIdQuery
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
            await _mediator.Send(command);
            return Ok("Evaluate created successfully");
        }
    }
}
