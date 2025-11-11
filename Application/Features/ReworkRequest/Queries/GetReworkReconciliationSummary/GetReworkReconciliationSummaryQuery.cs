using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkReconciliationSummary
{
    public class GetReworkReconciliationSummaryQuery : IRequest<ReconcilationSummaryDTO>
    {
        public Guid ReworkRequestId { get; set; }

        public GetReworkReconciliationSummaryQuery(Guid ReworkRequestId)
        {
            this.ReworkRequestId = ReworkRequestId;
        }
    }
}
