using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssignmentTransferRequest.Queries.GetAssignmentTransferForQcTransport
{
    public class GetAssignmentTransferForQcTransportQuery : IRequest<AssignmentTransferRequestDTO>
    {
        public Guid AssignmentTransferRequestId { get; set; }
    }
}
