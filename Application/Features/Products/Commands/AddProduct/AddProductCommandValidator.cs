using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IProductRepository _repository;

        public AddProductCommandValidator(IProductRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Product code is required.")
                .MaximumLength(50).WithMessage("Product code must not exceed 50 characters.")
                .MustAsync(async (code, cancellation) =>
                {
                    var isCodeUnique = await _repository.IsCodeUniqueAsync(code);
                    return isCodeUnique;
                }).WithMessage("Product code must be unique.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");
        }
    }
}
