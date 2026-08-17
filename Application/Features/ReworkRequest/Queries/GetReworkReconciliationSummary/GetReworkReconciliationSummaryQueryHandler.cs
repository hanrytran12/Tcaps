using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetReworkReconciliationSummary
{
    public class GetReworkReconciliationSummaryQueryHandler : IRequestHandler<GetReworkReconciliationSummaryQuery, ReconcilationSummaryDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetReworkReconciliationSummaryQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ReconcilationSummaryDTO> Handle(GetReworkReconciliationSummaryQuery request, CancellationToken cancellationToken)
        {
            var summaryData = await _appDbContext.ReworkRequests
                .AsNoTracking()
                .Where(r => r.AssignmentId == request.AssignmentId)
                .Select(r => new
                {
                    ReworkRequestId = r.Id,
                    QuantityTarget = r.DefectiveQuantity,

                    TotalSubmitted = _appDbContext.Productions
                        .Where(p => p.ReworkRequestId == r.Id)
                        .Sum(p => p.QuantityReceive),

                    TotalRejected = _appDbContext.Productions
                        .Where(p => p.ReworkRequestId == r.Id)
                        .SelectMany(p => _appDbContext.Evaluates.Where(e => e.ProductionId == p.Id))
                        .Where(e => e.Status == "Rejected")
                        .Sum(e => (int?)e.QuantityError) ?? 0,

                    TotalUnfixable = _appDbContext.Productions
                        .Where(p => p.ReworkRequestId == r.Id)
                        .SelectMany(p => _appDbContext.Evaluates.Where(e => e.ProductionId == p.Id))
                        .Where(e => e.Status == "Failed")
                        .SelectMany(e => _appDbContext.ComponentDefects.Where(cd => cd.EvaluateId == e.Id))
                        .Where(cd => cd.Status == "Unfixable")
                        .Sum(cd => (int?)cd.Quantity) ?? 0

                }).FirstOrDefaultAsync();

            if (summaryData == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu làm lại cho công đoạn này.");
            }

            var totalLoss = summaryData.TotalRejected + summaryData.TotalUnfixable;
            var finalCompletedQuantity = summaryData.TotalSubmitted - totalLoss;

            var materialSummaries = await _appDbContext.MaterialUses
                .AsNoTracking()
                .Where(m => m.ReworkRequestId == summaryData.ReworkRequestId)
                .Join(_appDbContext.Materials,
                      mu => mu.MaterialId,
                      ma => ma.Id,
                      (mu, ma) => new MaterialUsageSummaryDTO
                      {
                          MaterialId = ma.Id,
                          MaterialName = ma.Name,
                          QuantityDivided = mu.QuantityDivide,
                          QuantityStaffUsed = mu.QuantityStaffUse,
                          QuantityReconciled = mu.ReconciledQuantity
                      })
                .ToListAsync(cancellationToken);

            var summary = new ReconcilationSummaryDTO
            {
                ReworkRequestId = summaryData.ReworkRequestId,
                QuantityTarget = (int)summaryData.QuantityTarget,
                TotalSumbimttedQuantity = summaryData.TotalSubmitted,
                TotalRejectedQuantity = totalLoss,
                FinalCompletedQuantity = finalCompletedQuantity,
                MaterialUsageSummary = materialSummaries
            };

            return summary;
        }
    }
}
