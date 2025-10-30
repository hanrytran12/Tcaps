using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.ComponentDefect.Commands.UpdateComponentDefectConfirm
{
    public class UpdateComponentDefectConfirmCommand : IRequest<Result>
    {
        public Guid ComponentId { get; set; }
        public Guid EvaluateId { get; set; }
        public string Status { get; set; }
    }
}
