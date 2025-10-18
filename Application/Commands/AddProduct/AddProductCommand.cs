using Application.Common;
using MediatR;

namespace Application.Commands.AddProduct
{
    public class AddProductCommand : IRequest<Result<Guid>>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
    }
}
