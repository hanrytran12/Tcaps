using Application.DTOs;
using Application.Features.Productions.Command.AddProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/production")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _productionService;
        private readonly IMediator _mediator;

        public ProductionController(IProductionService productionService, IMediator mediator)
        {
            _productionService = productionService;
            _mediator = mediator;
        }

        [HttpPut("increase-production/{productionId:guid}")]
        public async Task<IActionResult> IncreaseProduction(Guid productionId, CancellationToken cancellationToken)
        {
            var response = await _productionService.IncreaseQuantityAsync(productionId, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("decrease-production/{productionId:guid}")]
        public async Task<IActionResult> DecreaseProduction(Guid productionId, CancellationToken cancellationToken)
        {
            var response = await _productionService.DecreaseQuantityAsync(productionId, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update-quantity/{productionId:guid}")]
        public async Task<IActionResult> UpdateQuantityProduction(Guid productionId, int newQuantity, CancellationToken cancellationToken)
        {
            var response = await _productionService.UpdateQuantityAsync(productionId, newQuantity, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("submit-request")]
        public async Task<IActionResult> SubmitProduction([FromBody]  AddProductionCommand command)
        {
            var result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllProductionQuery());
            return Ok(result);
        }

        [HttpGet("for-staff")]
        public async Task<IActionResult> GetByStaffIdAsync([FromQuery] GetAllProductionByStaffIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        public async Task<IActionResult> GetProductionsWithStatusPendingQC([FromQuery] GetAllProductionByQCIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
