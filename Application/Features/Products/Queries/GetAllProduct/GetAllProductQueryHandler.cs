using Application.DTOs.Response;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductsDTO>>
    {
        private readonly IProductRepository _repository;
        public GetAllProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductsDTO>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var listProduct = await _repository.GetAllAsync();
            var listProductDTO = listProduct.Select(p => new ProductsDTO
            {
                ProductId = p.Id,
                Code = p.Code,
                Name = p.Name,
                Image = p.Image,
                Description = p.Description,
            }).ToList();
            return listProductDTO;
        }
    }
}
