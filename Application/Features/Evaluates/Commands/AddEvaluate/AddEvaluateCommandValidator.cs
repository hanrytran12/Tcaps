using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommandValidator : AbstractValidator<AddEvaluateCommand>
    {
        public AddEvaluateCommandValidator()
        {
            RuleFor(x => x.QuantityError)
                .NotEmpty().WithMessage("QuantityError is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Note is required.")
                .MinimumLength(10).WithMessage("Note must be at least 10 characters long.")
                .MaximumLength(500).WithMessage("Note cannot exceed 500 characters.");
        }
    }
}
