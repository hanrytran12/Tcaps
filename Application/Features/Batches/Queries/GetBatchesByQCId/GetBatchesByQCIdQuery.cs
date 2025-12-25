using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchesByQCId
{
    public class GetBatchesByQCIdQuery : IRequest<List<BatchForQCDTO>>
    {
        public Guid QcId { get; set; }
    }
}
