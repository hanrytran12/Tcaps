using FluentValidation;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommandValidator : AbstractValidator<AddMaterialRequestCommand>
    {
        public AddMaterialRequestCommandValidator()
        {
            RuleFor(x => x.MaterialId)
                .NotEmpty().WithMessage("MaterialId is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("BatchId is required.");

            RuleFor(x => x.QuantityRequest)
                .GreaterThan(0).WithMessage("QuantityRequest must be greater than zero.");
        }
    }
}
