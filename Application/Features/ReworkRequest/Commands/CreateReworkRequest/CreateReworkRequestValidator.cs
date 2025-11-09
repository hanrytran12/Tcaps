using FluentValidation;

namespace Application.Features.ReworkRequest.Commands.CreateReworkRequest
{
    public class CreateReworkRequestValidator : AbstractValidator<CreateReworkRequestCommand>
    {
        public CreateReworkRequestValidator()
        {
            RuleFor(x => x.DefectiveQuantity).NotEmpty();
            RuleFor(x => x.NoteQc).NotEmpty();
        }
    }
}
