using FluentValidation;

namespace Application.Features.Assignments.Commands.AddAssignment
{
    public class AddAssignmentCommandValidator : AbstractValidator<AddAssignmentCommand.AddAssignmentCommand>
    {
        public AddAssignmentCommandValidator()
        {
            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("BatchId is required.");

            RuleFor(x => x.WorkshopId)
                .NotEmpty().WithMessage("WorkshopId is required.");

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity is required.")
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

            RuleFor(x => x.ExpectedDeliveryDate)
                .NotEmpty().WithMessage("Expected Delivery Date is required");
        }
    }
}
