using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductQuery : IRequest<List<ProductsDTO>> { }
}
