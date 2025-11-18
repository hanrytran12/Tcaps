using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQcTransport
{
    public class GetMaterialRequestForQcTransportQuery : IRequest<Result<Domain.Entities.MaterialRequest>>
    {
        public Guid MaterialRequestId { get; set; }
    }
}
