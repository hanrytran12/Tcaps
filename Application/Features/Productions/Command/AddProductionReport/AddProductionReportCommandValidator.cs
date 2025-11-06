using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.Productions.Command.AddProductionReport
{
    public class AddProductionReportCommandValidator : AbstractValidator<AddProductionReportCommand>
    {
        public AddProductionReportCommandValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
