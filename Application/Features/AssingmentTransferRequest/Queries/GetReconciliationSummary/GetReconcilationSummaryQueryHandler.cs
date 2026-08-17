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
            var summaryData = await _context.Assignments
                .AsNoTracking()
                .Where(a => a.Id == request.AssigmentId)
                .Select(a => new
                {
                    QuantityTarget = a.Quantity,

                    TotalSubmitted = _context.Productions
                    .Where(p => p.AssignId == a.Id)
                    .Sum(p => p.QuantityReceive),

                    TotalRejected = _context.Productions
                    .Where(p => p.AssignId == a.Id)
                    .SelectMany(p => _context.Evaluates.Where(e => e.ProductionId == p.Id))
                    .Where(e => e.Status == "Rejected")
                    .Sum(e => e.QuantityError),

                    TotalUnfixable = _context.Productions
                    .Where(p => p.AssignId == a.Id)
                    .SelectMany(p => _context.Evaluates.Where(e => e.ProductionId == p.Id))
                    .Where(e => e.Status == "Failed")
                    .SelectMany(e => _context.ComponentDefects.Where(cd => cd.EvaluateId == e.Id))
                    .Where(cd => cd.Status == "Unfixable")
                    .Sum(cd => cd.Quantity)

                }).FirstOrDefaultAsync();

            if (summaryData == null)
            {
                throw new Exception("Không tìm thấy công đoạn");
            }

            var totalLoss = summaryData.TotalRejected + summaryData.TotalUnfixable;
            var finalCompletedQuantity = summaryData.TotalSubmitted - totalLoss;

            var materialSummaries = await _context.MaterialUses
                .AsNoTracking()
                .Where(m => m.AssignId == request.AssigmentId)
                .Join(_context.Materials,
                    mu => mu.MaterialId,
                    ma => ma.Id,
                    (mu, ma) => new MaterialUsageSummaryDTO
                    {
                        MaterialId = ma.Id,
                        MaterialName = ma.Name,
                        QuantityDivided = mu.QuantityDivide + mu.QuantityRequest,
                        QuantityStaffUsed = mu.QuantityStaffUse,
                        QuantityReconciled = mu.ReconciledQuantity
                    })
                .ToListAsync();

            return new ReconcilationSummaryDTO
            {
                QuantityTarget = summaryData.QuantityTarget,
                TotalSumbimttedQuantity = summaryData.TotalSubmitted,
                TotalRejectedQuantity = totalLoss,
                FinalCompletedQuantity = finalCompletedQuantity,
                MaterialUsageSummary = materialSummaries
            };
        }
    }
}