using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetRequestById
{
    public class GetRequestByIdQuery : IRequest<Domain.Entities.ReworkRequest>
    {
        public Guid ReworkRequestId { get; set; }

        public GetRequestByIdQuery(Guid reworkRequestId)
        {
            ReworkRequestId = reworkRequestId;
        }
    }
}
