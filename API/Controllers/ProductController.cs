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
    public class ProductController : BaseApiController
    {
        public ProductController(ISender mediator) : base(mediator)
        {
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lead,QC,QCK,Staff")]
        public async Task<ActionResult<List<ProductsDTO>>> GetAllProduct()
        {
            var result = await Mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result, "Tạo sản phẩm thành công.");
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductCommand command)
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return HandleResult(result, "Cập nhật sản phẩm thành công.");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await Mediator.Send(new DeleteProductCommand(id));
            return HandleResult(result, "Xóa sản phẩm thành công.");
        }
    }
}
