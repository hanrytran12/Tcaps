using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Queries.GetAllBatch
{
    public class GetAllBatchQueryHandler : IRequestHandler<GetAllBatchQuery, List<Batch>>
    {
        private readonly IBatchRepository _batchRepository;

        public GetAllBatchQueryHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<List<Batch>> Handle(GetAllBatchQuery request, CancellationToken cancellationToken)
        {
            var batches = await _batchRepository.GetAllAsync();
            return batches.ToList();
        }
    }
}
