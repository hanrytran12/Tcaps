using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        public UpdateProductCommandHandler(IProductRepository repository, IFileStorageService fileStorageService)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new NotFoundException($"Không tìm thấy Product với Id: {request.Id}.");
            }

            if (request.Name != product.Name)
            {
                var existingProduct = await _repository.GetByNameAsync(request.Name);
                if (existingProduct is not null)
                {
                    throw new ConflictException("Tên sản phẩm này đã tồn tại");
                }
            }

            var relativePath = product.Image;
            if (request.ImageFile is not null)
            {
                await _fileStorageService.DeleteFileAsync(product.Image, cancellationToken);
                relativePath = await _fileStorageService.SaveFileAsync(request.ImageFile, "products", cancellationToken);
            }

            product.UpdateDetails(request.Name, request.Description, relativePath);
            return Result.Success();
        }
    }
}
