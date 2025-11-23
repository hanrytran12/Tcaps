using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkForDashboard
{
    public class GetReworkForDashboardQuery : IRequest<Domain.Entities.ReworkRequest>
    {
        public Guid AssignmentId { get; set; }

        public GetReworkForDashboardQuery(Guid assignmentId)
        {
            AssignmentId = assignmentId;
        }
    }
}
