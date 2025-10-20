using Application.Commands.AddBatch;
using Application.Commands.DeleteBatch;
using Application.Commands.UpdateBatch;
using Application.Queries.GetAllBatch;
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
    }
}
