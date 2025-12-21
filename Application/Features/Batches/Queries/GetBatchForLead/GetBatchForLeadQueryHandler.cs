using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQueryHandler : IRequestHandler<GetBatchForLeadQuery, List<Batch>>
    {
        private readonly IBatchRepository _context;

        public GetBatchForLeadQueryHandler(IBatchRepository context)
        {
            _context = context;
        }
        public async Task<List<Batch>> Handle(GetBatchForLeadQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.GetBatchesByLeadIdAsync(request.UserId);
            return list;
        }
    }
}
