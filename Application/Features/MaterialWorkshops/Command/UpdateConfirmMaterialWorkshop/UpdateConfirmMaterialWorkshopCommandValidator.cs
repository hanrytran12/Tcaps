using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop
{
    public class UpdateConfirmMaterialWorkshopCommandValidator : AbstractValidator<UpdateConfirmMaterialWorkshopCommand>
    {
        public UpdateConfirmMaterialWorkshopCommandValidator()
        {
            RuleFor(x => x.QuantityReceive)
                .NotEmpty().WithMessage("QuantityReceive is required.")
                .GreaterThan(0).WithMessage("QuantityReceive must be greater than zero.");
        }
    }
}
