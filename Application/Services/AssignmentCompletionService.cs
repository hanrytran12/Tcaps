using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AssignmentCompletionService : IAssignmentCompletionService
    {
        private readonly IAppDbContext _appDbContext;
        public AssignmentCompletionService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<decimal> CalculateCompetedQuantityAsync(Guid assignmentId)
        {
            var totalQuantity = await _appDbContext.Productions.Where(p => p.AssignId == assignmentId).SumAsync(p => p.Quantity);

            var query = from p in _appDbContext.Productions
                        where p.AssignId == assignmentId
                        join e in _appDbContext.Evaluates on p.Id equals e.ProductionId
                        where e.Status == "Rejected"
                        select e.QuantityError;

            var totalReject = await query.SumAsync();

            return totalQuantity - totalReject;
        }
    }
}
