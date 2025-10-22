using Application.Features.Batches.Commands.AddBatch;
using Application.Features.Batches.Commands.DeleteBatch;
using Application.Features.Batches.Commands.UpdateBatch;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetDashboardStats;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> AddBatch(AddBatchCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBatch(Guid id, UpdateBatchCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBatch(Guid id)
        {
            var command = new DeleteBatchCommand(id);
            var result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : BadRequest(result.error);
        }


        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats([FromQuery] GetDashboardStatsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
