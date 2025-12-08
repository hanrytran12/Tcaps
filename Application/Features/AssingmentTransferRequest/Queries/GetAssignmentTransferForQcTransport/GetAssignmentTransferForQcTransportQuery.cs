using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport
{
    public class GetAssignmentTransferForQcTransportQuery : IRequest<Result<AssignmentTransferRequestDTO>>
    {
        public Guid AssignmentTransferRequestId { get; set; }
    }
}
