using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest
{
    public class CreateTaskTransferRequestCommandValidator : AbstractValidator<CreateTaskTransferRequestCommand>
    {
        public CreateTaskTransferRequestCommandValidator()
        {
            RuleFor(x => x.DateToGo)
                .NotEmpty().WithMessage("DateToGo is required.");
        }
    }
}
