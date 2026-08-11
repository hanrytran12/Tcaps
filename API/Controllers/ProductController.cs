using Application.DTOs.Response;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;
        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,Staff")]
        public async Task<List<ProductsDTO>> GetAllProduct()
        {
            return await _sender.Send(new GetAllProductQuery());
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductCommand command)
        {
            await _sender.Send(command);
            return Ok("Product added successfully");
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductCommand command)
        {
            command.Id = id;
            await _sender.Send(command);
            return Ok("Product updated successfully");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _sender.Send(new DeleteProductCommand(id));
            return Ok("Product deleted successfully");
        }
    }
}
