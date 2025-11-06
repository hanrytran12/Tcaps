using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop
{
    public class AddMaterialWorkshopCommandValidator : AbstractValidator<AddMaterialWorkshopCommand>
    {
        public AddMaterialWorkshopCommandValidator()
        {
            RuleFor(x => x.QuantitySend)
                .NotEmpty().WithMessage("QuantitySend is required.")
                .GreaterThan(0).WithMessage("QuantitySend must be greater than zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required.")
                .MaximumLength(50).WithMessage("Unit cannot exceed 50 characters.");
        }
    }
}
