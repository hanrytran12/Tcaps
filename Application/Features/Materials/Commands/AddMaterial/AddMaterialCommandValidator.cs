using FluentValidation;

namespace Application.Features.Materials.Commands.AddMaterial
{
    public class AddMaterialCommandValidator : AbstractValidator<AddMaterialCommand>
    {
        public AddMaterialCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Unit).NotEmpty().WithMessage("Unit is required.");
        }
    }
}
