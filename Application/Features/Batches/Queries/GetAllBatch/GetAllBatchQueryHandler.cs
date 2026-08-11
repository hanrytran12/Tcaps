using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Application.Features.Batches.Mapping;
using Application.DTOs.Response;

namespace Application.Features.Batches.Queries.GetAllBatch
{
    public class GetAllBatchQueryHandler : IRequestHandler<GetAllBatchQuery, List<BatchResponseDTO>>
    {
        private readonly IBatchRepository _batchRepository;

        public GetAllBatchQueryHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<List<BatchResponseDTO>> Handle(GetAllBatchQuery request, CancellationToken cancellationToken)
        {
            var batches = await _batchRepository.GetAllAsync();
            return batches.Select(BatchResponseMapper.ToResponse).ToList();
        }
    }
}
