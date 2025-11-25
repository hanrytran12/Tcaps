using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkForDashboard
{
    public class GetReworkForDashboardQuery : IRequest<ReworkRequestDTO>
    {
        public Guid AssignmentId { get; set; }

        public GetReworkForDashboardQuery(Guid assignmentId)
        {
            AssignmentId = assignmentId;
        }
    }
}
