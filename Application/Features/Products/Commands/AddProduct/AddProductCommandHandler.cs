using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public AddProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public async Task<Result<Guid>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = string.Empty;
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                string uploadPath = Path.Combine(_environment.WebRootPath, "images", "products");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.ImageFile.FileName;
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }
                imageUrl = $"/images/products/{uniqueFileName}";
            }

            var product = Product.Create(request.Code, request.Name, imageUrl, request.Description);
            await _repository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(product.Id);
        }
    }
}
