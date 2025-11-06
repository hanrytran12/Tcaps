using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductsDTO>>
    {
        private readonly IProductRepository _repository;
        private readonly IFileStorageService _fileStorageService;

        public GetAllProductQueryHandler(IProductRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<ProductsDTO>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var listProduct = await _repository.GetAllAsync();
            var listProductDTO = listProduct.Select(p => new ProductsDTO
            {
                ProductId = p.Id,
                Code = p.Code,
                Name = p.Name,
                Image = _fileStorageService.GetFileUrl(p.Image),
                Description = p.Description,
            }).ToList();
            return listProductDTO;
        }
    }
}
