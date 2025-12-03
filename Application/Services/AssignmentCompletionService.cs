using Application.DTOs.Response;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AssignmentCompletionService : IAssignmentCompletionService
    {
        private readonly IAppDbContext _context;
        public AssignmentCompletionService(IAppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<SummaryCalculateCompletedDTO> CalculateCompetedQuantityAsync(Guid assignmentId, Guid? reworkRequestId)
        {
            if (reworkRequestId == null)
            {
                var summaryData = await _context.Assignments
                .AsNoTracking()
                .Where(a => a.Id == assignmentId)
                .Select(a => new
                {
                    TotalSubmitted = _context.Productions
                    .Where(p => p.AssignId == a.Id)
                    .Sum(p => p.Quantity),

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
                return new SummaryCalculateCompletedDTO
                {
                    TotalSubmitted = summaryData.TotalSubmitted,
                    TotalCompleted = summaryData.TotalSubmitted - totalLoss,
                    TotalRejected = totalLoss,
                };
            }
            else
            {
                var summaryData = await _context.Assignments
                .AsNoTracking()
                .Where(a => a.Id == assignmentId)
                .Select(a => new
                {
                    TotalSubmitted = _context.Productions
                    .Where(p => p.AssignId == a.Id && p.ReworkRequestId == reworkRequestId)
                    .Sum(p => p.Quantity),

                    TotalRejected = _context.Productions
                    .Where(p => p.AssignId == a.Id && p.ReworkRequestId == reworkRequestId)
                    .SelectMany(p => _context.Evaluates.Where(e => e.ProductionId == p.Id))
                    .Where(e => e.Status == "Rejected")
                    .Sum(e => e.QuantityError),

                    TotalUnfixable = _context.Productions
                    .Where(p => p.AssignId == a.Id && p.ReworkRequestId == reworkRequestId)
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
                return new SummaryCalculateCompletedDTO
                {
                    TotalSubmitted = summaryData.TotalSubmitted,
                    TotalCompleted = summaryData.TotalSubmitted - totalLoss,
                    TotalRejected = totalLoss,
                };
            }
        }
    }
}
