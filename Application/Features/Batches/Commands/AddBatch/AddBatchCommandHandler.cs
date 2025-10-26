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

        public AddBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<Guid>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByCodeAsync(request.CodeProduct);
            if (product is null)
            {
                return Result<Guid>.Failure("Product is not exist.");
            }

            var batch = Batch.Create(
                product.Id,
                request.Code,
                request.Quantity,
                request.StartDate,
                request.EndDate
            );
            await _batchRepository.AddAsync(batch);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(batch.Id);
        }
    }
}
