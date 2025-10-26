using FluentValidation;

namespace Application.Features.MaterialUse.Commands.AddMaterialUse
{
    public class AddMaterialUseCommandValidator : AbstractValidator<AddMaterialUseCommand>
    {
        public AddMaterialUseCommandValidator()
        {
            RuleFor(x => x.MaterialId)
                .NotEmpty().WithMessage("MaterialId is required.");

            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("BatchId is required.");

            RuleFor(x => x.AssignId)
                .NotEmpty().WithMessage("AssignId is required.");

            RuleFor(x => x.QuantityDivide)
                .NotEmpty().WithMessage("QuantityDivide is required.")
                .GreaterThan(0).WithMessage("QuantityDivide must be greater than zero.");
        }
    }
}
