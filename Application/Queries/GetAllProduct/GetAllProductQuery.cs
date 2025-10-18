using Domain.Entities;
using MediatR;

namespace Application.Queries.GetAllProduct
{
    public class GetAllProductQuery : IRequest<List<Product>> { }
}
