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
        }
    }
}
