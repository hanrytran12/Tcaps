using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssignmentTransferRequest.Queries.GetAllForQcTransport
{
    public class GetAllForQcTransportQuery : IRequest<List<AssignmentTransferRequestDTO>>
    {
        public Guid QcTransportId { get; set; }
    }
}
