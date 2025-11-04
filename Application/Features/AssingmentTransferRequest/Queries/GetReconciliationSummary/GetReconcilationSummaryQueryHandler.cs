using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetReconciliationSummary
{
    public class GetReconciliationSummaryQueryHandler : IRequestHandler<GetReconciliationSummaryQuery, ReconcilationSummaryDTO>
    {
        private readonly IAppDbContext _context;
        public GetReconciliationSummaryQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<ReconcilationSummaryDTO> Handle(GetReconciliationSummaryQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _context.Assignments.AsNoTracking().Select(a => new { a.Id, a.Quantity }).FirstOrDefaultAsync(a => a.Id == request.AssigmentId);
            if (assignment == null)
            {
                throw new Exception("Không tìm thấy công đoạn.");
            }

            var totalSubmitted = await _context.Productions.AsNoTracking().Where(p => p.AssignId == request.AssigmentId).SumAsync(p => p.Quantity);

            var query = from p in _context.Productions
                        where p.AssignId == request.AssigmentId
                        join e in _context.Evaluates on p.Id equals e.ProductionId
                        where e.Status == "Rejected"
                        select e.QuantityError;

            var totalRejected = await query.SumAsync();
            var finalCompletedQuantity = totalSubmitted - totalRejected;

            var materialSummaries = await _context.MaterialUse.AsNoTracking().Where(m => m.AssignId == request.AssigmentId)
                                                  .Join(_context.Materials,
                                                  mu => mu.MaterialId,
                                                  ma => ma.Id,
                                                  (mu, ma) => new MaterialUsageSummaryDTO
                                                  {
                                                      MaterialId = ma.Id,
                                                      MaterialName = ma.Name,
                                                      QuantityDivided = mu.QuantityDivide,
                                                      QuantityStaffUsed = mu.QuantityStaffUse
                                                  }).ToListAsync();

            var summary = new ReconcilationSummaryDTO
            {
                QuantityTarget = assignment.Quantity,
                TotalSumbimttedQuantity = totalSubmitted,
                TotalRejectedQuantity = totalRejected,
                FinalCompletedQuantity = finalCompletedQuantity,
                MaterialUsageSummary = materialSummaries
            };

            return summary;
        }
    }
}