using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest
{
    public class GetAllTransferRequestQuery : IRequest<List<AssignmentTransferRequestDTO>>
    {
    }
}
