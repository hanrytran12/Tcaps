using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Application.Features.Batches.Mapping;
using Application.DTOs.Response;

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQueryHandler : IRequestHandler<GetBatchForLeadQuery, List<BatchResponseDTO>>
    {
        private readonly IBatchRepository _context;

        public GetBatchForLeadQueryHandler(IBatchRepository context)
        {
            _context = context;
        }
        public async Task<List<BatchResponseDTO>> Handle(GetBatchForLeadQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.GetBatchesByLeadIdAsync(request.UserId);
            return list.Select(BatchResponseMapper.ToResponse).ToList();
        }
    }
}
