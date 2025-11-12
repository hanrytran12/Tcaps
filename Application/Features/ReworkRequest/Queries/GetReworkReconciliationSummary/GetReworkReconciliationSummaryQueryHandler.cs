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
            var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.Id == request.ReworkRequestId).FirstOrDefaultAsync(cancellationToken);

            var totalSubmitted = await _appDbContext.Productions.AsNoTracking().Where(p => p.AssignId == reworkRequest.AssignmentId && p.ReworkRequestId == request.ReworkRequestId).SumAsync(p => p.Quantity);

            var query = from p in _appDbContext.Productions
                        where p.AssignId == reworkRequest.AssignmentId && p.ReworkRequestId == request.ReworkRequestId
                        join e in _appDbContext.Evaluates on p.Id equals e.ProductionId
                        where e.Status == "Rejected"
                        select e.QuantityError;

            var totalRejected = await query.SumAsync();
            var finalCompletedQuantity = totalSubmitted - totalRejected;

            var materialSummaries = await _appDbContext.MaterialUse.AsNoTracking().Where(m => m.ReworkRequestId == request.ReworkRequestId)
                                                  .Join(_appDbContext.Materials,
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
                QuantityTarget = (int)reworkRequest.DefectiveQuantity,
                TotalSumbimttedQuantity = totalSubmitted,
                TotalRejectedQuantity = totalRejected,
                FinalCompletedQuantity = finalCompletedQuantity,
                MaterialUsageSummary = materialSummaries
            };

            return summary;
        }
    }
}
