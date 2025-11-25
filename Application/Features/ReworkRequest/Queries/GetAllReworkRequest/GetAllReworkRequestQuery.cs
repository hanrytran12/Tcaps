using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetAllReworkRequest
{
    public class GetAllReworkRequestQuery : IRequest<List<ReworkRequestDTO>>
    {
    }
}
