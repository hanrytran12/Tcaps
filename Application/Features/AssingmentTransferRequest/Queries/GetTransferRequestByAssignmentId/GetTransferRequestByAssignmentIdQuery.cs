using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetTransferRequestByAssignmentId
{
    public class GetTransferRequestByAssignmentIdQuery : IRequest<TransferRequestDTO>
    {
        public Guid AssignmentId { get; set; }

        public GetTransferRequestByAssignmentIdQuery(Guid assigmentId)
        {
            AssignmentId = assigmentId;
        }
    }
}
