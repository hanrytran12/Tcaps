using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/production")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionService _productionService;

        public ProductionController(IProductionService productionService)
        {
            _productionService = productionService;
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
        public async Task<IActionResult> SubmitProduction([FromBody] ProductionDTO dto, int newQuantity, CancellationToken cancellationToken)
        {
            var response = await _productionService.SubmitProductionAsync(dto, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
