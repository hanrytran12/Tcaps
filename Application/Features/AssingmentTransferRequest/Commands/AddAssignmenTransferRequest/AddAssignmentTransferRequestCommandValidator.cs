using FluentValidation;

namespace Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest
{
    public class AddAssignmentTransferRequestCommandValidator : AbstractValidator<AddAssignmentTransferRequestCommand>
    {
        public AddAssignmentTransferRequestCommandValidator()
        {
            RuleFor(x => x.AssignmentId).NotEmpty().WithMessage("Assignment is required");
        }
    }
}
