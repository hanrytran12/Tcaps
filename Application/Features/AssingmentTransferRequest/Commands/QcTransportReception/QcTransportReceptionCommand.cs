using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.QcTransportReception
{
    public class QcTransportReceptionCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid QCTransportId { get; set; }
        public Guid AssignmentTransferRequestId { get; set; }
    }
}
