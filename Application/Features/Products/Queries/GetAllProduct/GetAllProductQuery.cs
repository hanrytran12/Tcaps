using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductQuery : IRequest<List<Product>> { }
}
