using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchById;
using Application.Features.Batches.Queries.GetBatchByWorkshopId;
using Application.Features.Batches.Queries.GetBatchesByQCId;
using Application.Features.Batches.Queries.GetBatchesByStaffId;
using Application.Features.Batches.Queries.GetDashboardStats;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BatchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<Batch>> GetAllBatch()
        {
            var listBatch = await _mediator.Send(new GetAllBatchQuery());
            return listBatch;
        }

        [HttpGet("{batchId:guid}")]
        public async Task<IActionResult> GetBatchById(Guid batchId)
        {
            var query = new GetBatchByIdQuery(batchId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("dashboard")]
        [Authorize(Policy = "CanViewDashboard")]
        public async Task<IActionResult> GetDashboardStats([FromQuery] GetDashboardStatsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetBatchByWorkshopId([FromQuery] string? Status, DateOnly? FromDate, DateOnly? ToDate)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var userId = Guid.Parse(userIdString);

            var query = new GetBatchByWorkshopIdQuery(userId, Status, FromDate, ToDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddBatch(AddBatchCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return CreatedAtAction(nameof(GetAllBatch), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateBatch(Guid id, UpdateBatchCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteBatch(Guid id)
        {
            var command = new DeleteBatchCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpGet("staff/bactches")]
        public async Task<IActionResult> GetBatchesByStaffIdAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var query = new GetBatchesByStaffIdQuery
            {
                StaffId = Guid.Parse(userIdString)
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }

        [HttpGet("qc/bactches")]
        [Authorize(Policy = "QC")]
        public async Task<IActionResult> GetBatchesByQCIdAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không thể xác định người dùng từ token.");
            }

            var query = new GetBatchesByQCIdQuery
            {
                QcId = Guid.Parse(userIdString)
            };

            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result) : BadRequest(result.IsFailure);
        }
    }
}
