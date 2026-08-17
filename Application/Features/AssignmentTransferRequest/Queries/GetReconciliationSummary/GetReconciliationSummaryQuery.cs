using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssignmentTransferRequest.Queries.GetReconciliationSummary
{
    public class GetReconciliationSummaryQuery : IRequest<ReconcilationSummaryDTO>
    {
        public Guid AssigmentId { get; set; }

        public GetReconciliationSummaryQuery(Guid assigmentId)
        {
            AssigmentId = assigmentId;
        }
    }
}