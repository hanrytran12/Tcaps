using Application.Common;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, Result<Guid>>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private const string CodeBatch = "LO_";

        public AddBatchCommandHandler(IBatchRepository batchRepository, IProductRepository productRepository, IMediator mediator, IUserRepository userRepository)
        {
            _batchRepository = batchRepository;
            _productRepository = productRepository;
            _mediator = mediator;
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var lastIndex = await _batchRepository.GetLastCodeIndexAsync(CodeBatch);
            var nextIndex = (lastIndex ?? 0) + 1;
            var newCode = $"{CodeBatch}{nextIndex}";

            var product = await _productRepository.GetByCodeAsync(request.CodeProduct);
            if (product is null)
            {
                throw new NotFoundException("Product is not exist.");
            }

            var lead = await _userRepository.GetByIdAsync(request.UserId);
            if (lead is null || lead.Role != "Lead")
            {
                throw new NotFoundException("Đây không phải là Lead.");
            }

            var result = Batch.Create(
                product.Id,
                request.UserId,
                newCode,
                request.Quantity,
                request.StartDate,
                request.EndDate
            );
            await _batchRepository.AddAsync(result);
            await _mediator.Publish(new AddBatchEvent(request.UserId, newCode, request.Quantity));
            return Result<Guid>.Success(result.Id);
        }
    }
}
