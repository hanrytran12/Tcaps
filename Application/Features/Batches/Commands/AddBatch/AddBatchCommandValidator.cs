using FluentValidation;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandValidator : AbstractValidator<AddBatchCommand>
    {
        public AddBatchCommandValidator()
        {
            RuleFor(x => x.CodeProduct)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Batch code is required.")
                .MaximumLength(50).WithMessage("Batch code must not exceed 50 characters.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.ImageFile)
                .NotEmpty().WithMessage("Image is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
