using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        private const string ProductPrefix = "NON_";

        public AddProductCommandHandler(IProductRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Guid>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (!await _repository.IsNameUniqueAsync(request.Name))
            {
                return Result<Guid>.Failure("Tên sản phẩm này đã tồn tại");
            }

            var lastIndex = await _repository.GetLastCodeIndexAsync(ProductPrefix);
            var nextIdex = (lastIndex ?? 0) + 1;
            var newCode = $"{ProductPrefix}{nextIdex}";

            string realativePath = await _fileStorageService.SaveFileAsync(request.ImageFile, "products", cancellationToken);

            var product = Product.Create(newCode, request.Name, realativePath, request.Description);
            await _repository.AddAsync(product);
            return Result<Guid>.Success(product.Id);
        }
    }
}
