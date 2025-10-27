using FluentValidation;

namespace Application.Features.Batches.Commands.UpdateBatch
{
    public class UpdateBatchCommandValidator : AbstractValidator<UpdateBatchCommand>
    {
        public UpdateBatchCommandValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");
            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
