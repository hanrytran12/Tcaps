using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
