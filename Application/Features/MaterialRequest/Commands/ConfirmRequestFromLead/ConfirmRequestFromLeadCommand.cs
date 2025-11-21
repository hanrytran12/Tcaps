using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromLead
{
    public class ConfirmRequestFromLeadCommand : IRequest<Result<Guid>>
    {
        public Guid MaterialRequest { get; set; }
    }
}
