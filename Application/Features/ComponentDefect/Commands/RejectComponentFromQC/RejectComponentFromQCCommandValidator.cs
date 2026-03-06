using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Features.ComponentDefect.Commands.RejectComponentFromQC
{
    public class RejectComponentFromQCCommandValidator : AbstractValidator<RejectComponentFromQCCommand>
    {
        public RejectComponentFromQCCommandValidator()
        {
            RuleFor(x => x.QuantityReject)
                .GreaterThan(0).WithMessage("Số lượng từ chối phải lớn hơn 0.");
        }
    }
}
