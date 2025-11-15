using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkReconciliationSummary
{
    public class GetReworkReconciliationSummaryQuery : IRequest<ReconcilationSummaryDTO>
    {
        public Guid AssignmentId { get; set; }

        public GetReworkReconciliationSummaryQuery(Guid assignmentId)
        {
            AssignmentId = assignmentId;
        }
    }
}
