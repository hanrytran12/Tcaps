using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandValidator : AbstractValidator<AddBatchCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBatchRepository _batchRepository;
        public AddBatchCommandValidator(IBatchRepository batchRepository, IProductRepository productRepository)
        {
            _batchRepository = batchRepository;
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .MustAsync(async (productId, cancellation) =>
                {
                    var product = await _productRepository.GetByIdAsync(productId);
                    return product is not null;
                }).WithMessage("Product ID must refer to an existing production.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Batch code is required.")
                .MaximumLength(50).WithMessage("Batch code must not exceed 50 characters.")
                .MustAsync(async (code, cancellation) =>
                {
                    var existingBatch = await _batchRepository.GetByCodeAsync(code);
                    return existingBatch is null;
                }).WithMessage("Batch code must be unique.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
