using Application.Features.Productions.Command.AddProductionReport;
using Application.Features.Productions.Command.UpdateProduction;
using Application.Features.Productions.Query.GetAllProduction;
using Application.Features.Productions.Query.GetAllProductionByQCId;
using Application.Features.Productions.Query.GetAllProductionByStaffId;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        //[HttpPost("submit-request")]
        //public async Task<IActionResult> SubmitProduction([FromBody] AddProductionCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        //}

        [HttpPost("report-work")]
        public async Task<IActionResult> ReportWork([FromBody] AddProductionReportCommand command)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            command.StaffId = userId;
            var result = await _mediator.Send(command);
            return (result.IsSuccess) ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllProductionQuery());
            return Ok(result);
        }

        [HttpGet("for-staff")]
        public async Task<IActionResult> GetByStaffIdAsync([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAllProductionByStaffIdQuery
            {
                UserId = Guid.Parse(userIdString),
                Status = status
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("for-qc")]
        public async Task<IActionResult> GetProductionsWithStatusPendingQC([FromQuery] string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized();
            }

            var query = new GetAllProductionByQCIdQuery
            {
                QC_Id = Guid.Parse(userIdString),
                Status = status
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("for-qc/reduce-quantity")]
        [Authorize(Roles = "QC")]
        public async Task<IActionResult> UpdateQuantity([FromQuery] UpdateProductionCommand command)
        {
            var result = await _mediator.Send(command);
            return (result.IsSuccess) ? Ok(result) : BadRequest(result);
        }
    }
}
