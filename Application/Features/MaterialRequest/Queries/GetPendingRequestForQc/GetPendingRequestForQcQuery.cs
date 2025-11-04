using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetPendingRequestForQc
{
    public class GetPendingRequestForQcQuery : IRequest<List<PendingRequestDTO>>
    {
        public Guid QcId { get; set; }

        public GetPendingRequestForQcQuery(Guid qcId)
        {
            QcId = qcId;
        }
    }
}
