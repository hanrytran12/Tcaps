using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.ComponentDefect.Commands.RejectComponentFromQC
{
    public class RejectComponentFromQCCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid EvaluateId { get; set; }
        public int QuantityReject { get; set; }
    }
}
