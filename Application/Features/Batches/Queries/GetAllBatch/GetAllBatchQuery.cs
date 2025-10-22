using Domain.Entities;
using MediatR;

namespace Application.Features.Batches.Queries.GetAllBatch
{
    public class GetAllBatchQuery : IRequest<List<Batch>>
    {
    }
}
