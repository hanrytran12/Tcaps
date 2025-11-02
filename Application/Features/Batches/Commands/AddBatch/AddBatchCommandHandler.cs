using Application.Common;
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
        private const string CodeBatch = "LO_";

        public AddBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<Guid>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var lastIndex = await _batchRepository.GetLastCodeIndexAsync(CodeBatch);
            var nextIndex = (lastIndex ?? 0) + 1;
            var newCode = $"{CodeBatch}{nextIndex}";

            var product = await _productRepository.GetByCodeAsync(request.CodeProduct);
            if (product is null)
            {
                return Result<Guid>.Failure("Product is not exist.");
            }

            var result = Batch.Create(
                product.Id,
                newCode,
                request.Quantity,
                request.StartDate,
                request.EndDate
            );
            await _batchRepository.AddAsync(result);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(result.Id);
        }
    }
}
