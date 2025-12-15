using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByAssignId
{
    public class GetReworkByAssignIdQuery : IRequest<Domain.Entities.ReworkRequest>
    {
        public Guid AssignId { get; set; }
    }
}
