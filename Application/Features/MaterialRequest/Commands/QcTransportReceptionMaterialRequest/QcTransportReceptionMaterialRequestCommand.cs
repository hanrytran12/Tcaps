using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.QcTransportReceptionMaterialRequest
{
    public class QcTransportReceptionMaterialRequestCommand : IRequest<Result<Guid>>
    {
        public Guid QcTransportId { get; set; }
        public Guid MaterialRequestId { get; set; }
    }
}
