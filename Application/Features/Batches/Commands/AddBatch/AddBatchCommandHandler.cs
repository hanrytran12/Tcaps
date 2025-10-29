using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;

        public AddBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository, IProductRepository productRepository, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
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

            var imageUrl = await _fileStorageService.SaveFileAsync(request.ImageFile, "batches", cancellationToken);

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
