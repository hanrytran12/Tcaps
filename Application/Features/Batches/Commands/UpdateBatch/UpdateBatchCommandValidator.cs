using FluentValidation;

namespace Application.Features.Batches.Commands.UpdateBatch
{
    public class UpdateBatchCommandValidator : AbstractValidator<UpdateBatchCommand>
    {
        public UpdateBatchCommandValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
