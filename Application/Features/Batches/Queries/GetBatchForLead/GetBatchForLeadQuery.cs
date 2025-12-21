using Domain.Entities;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQuery : IRequest<List<Batch>>
    {
        public Guid UserId { get; set; }
    }
}
