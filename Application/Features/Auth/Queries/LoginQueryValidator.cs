using FluentValidation;

namespace Application.Features.Auth.Queries
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.EmailOrPhone)
                .NotEmpty().WithMessage("EmailOrPhone is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
