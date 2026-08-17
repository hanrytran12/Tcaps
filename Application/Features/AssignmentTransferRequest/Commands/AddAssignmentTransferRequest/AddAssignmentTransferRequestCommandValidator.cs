using FluentValidation;

namespace Application.Features.AssignmentTransferRequest.Commands.AddAssignmentTransferRequest
{
    public class AddAssignmentTransferRequestCommandValidator : AbstractValidator<AddAssignmentTransferRequestCommand>
    {
        public AddAssignmentTransferRequestCommandValidator()
        {
            RuleFor(x => x.AssignmentId).NotEmpty().WithMessage("Assignment is required");
        }
    }
}
