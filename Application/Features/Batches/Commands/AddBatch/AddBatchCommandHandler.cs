using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository, IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<Result<Guid>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByCodeAsync(request.CodeProduct);
            if (product is null)
            {
                return Result<Guid>.Failure("Product is not exist.");
            }

            var batch = await _batchRepository.GetByCodeAsync(request.Code);
            if (batch is not null)
            {
                return Result<Guid>.Failure("Batch code is not unique.");
            }

            string imageUrl = string.Empty;
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "batches");

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

            var result = Batch.Create(
                product.Id,
                request.Code,
                request.Quantity,
                imageUrl,
                request.StartDate,
                request.EndDate
            );
            await _batchRepository.AddAsync(result);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(result.Id);
        }
    }
}
