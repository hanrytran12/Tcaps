using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetAllBatch
{
    public class GetAllBatchQueryHandler : IRequestHandler<GetAllBatchQuery, List<Batch>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;

        public GetAllBatchQueryHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task<List<Batch>> Handle(GetAllBatchQuery request, CancellationToken cancellationToken)
        {
            var batches = await _batchRepository.GetAllAsync();
            return batches.ToList();
        }
    }
}
