using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<Product>>
    {
        private readonly IProductRepository _repository;
        public GetAllProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var listProduct = await _repository.GetAllAsync();
            return listProduct.ToList();
        }
    }
}
