using FluentValidation;

namespace Application.Features.Inventories.Commands.AddInventory
{
    public class AddInventoryCommandValidator : AbstractValidator<AddInventoryCommand>
    {
        public AddInventoryCommandValidator()
        {
            RuleFor(x => x.MaterialId)
                .NotEmpty().WithMessage("MaterialId is required.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.ImageURL)
                .NotNull().WithMessage("ImageURL is required.");
        }
    }
}
