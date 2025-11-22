using FluentValidation;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommandValidator : AbstractValidator<AddEvaluateCommand>
    {
        public AddEvaluateCommandValidator()
        {

            RuleFor(x => x.QuantityError)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal zero.");

            RuleFor(x => x.QuantitySucess)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal zero.");

            //RuleFor(x => x.QuantityError)
            //    //.NotEmpty().WithMessage("QuantityError is required.")
            //    .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal zero.");


            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Note is required.");
        }
    }
}
