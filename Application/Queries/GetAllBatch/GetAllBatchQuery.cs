using Domain.Entities;
using MediatR;

namespace Application.Queries.GetAllBatch
{
    public class GetAllBatchQuery : IRequest<List<Batch>>
    {
    }
}
