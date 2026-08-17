using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.FinalTransferRequest.Commands.ApproveFinalTransferRequest
{
    public class ApproveFinalTransferRequestCommandValidator : AbstractValidator<ApproveFinalTransferRequestCommand>
    {
        public ApproveFinalTransferRequestCommandValidator()
        {
            RuleFor(f => f.QuantityFinalReceive)
                .NotEmpty().WithMessage("Số lượng thực nhận không được để trống");

            RuleFor(f => f.Note)
                .NotEmpty().WithMessage("Ghi chú không được để trống.");
        }
    }
}
