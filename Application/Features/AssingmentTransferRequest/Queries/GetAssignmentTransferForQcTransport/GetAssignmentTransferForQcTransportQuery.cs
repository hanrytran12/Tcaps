using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport
{
    public class GetAssignmentTransferForQcTransportQuery : IRequest<AssignmentTransferRequestDTO>
    {
        public Guid AssignmentTransferRequestId { get; set; }
    }
}
