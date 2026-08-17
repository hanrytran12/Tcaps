using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetRequestById
{
    public class GetRequestByIdQuery : IRequest<Application.DTOs.Response.ReworkRequestResponseDTO>
    {
        public Guid ReworkRequestId { get; set; }

        public GetRequestByIdQuery(Guid reworkRequestId)
        {
            ReworkRequestId = reworkRequestId;
        }
    }
}
