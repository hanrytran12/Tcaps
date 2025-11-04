using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AppDbContext _context;
        public BatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Batch batch)
        {
            await _context.AddAsync(batch);
        }

        public void Delete(Batch batch)
        {
            _context.Remove(batch);
        }

        public async Task<IEnumerable<Batch>> GetAllAsync()
        {
            return await _context.Batches.Where(b => !b.isDeleted).ToListAsync();
        }

        public async Task<Batch?> GetByIdAsync(Guid Id)
        {
            return await _context.Batches.FirstOrDefaultAsync(b => b.Id == Id);
        }

        public void Update(Batch batch)
        {
            _context.Update(batch);
        }

        public async Task<Batch?> GetByIdWithAssignmentsAsync(Guid Id)
        {
            return await _context.Batches
                                 .Include(b => b.Assignments)
                                 .FirstOrDefaultAsync(b => b.Id == Id);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByIdsAsync(List<Guid> ids)
        {
            return await _context.Batches
                .AsNoTracking()
                .Where(b => ids.Contains(b.Id))
                .ToListAsync();
        }

        public async Task<Batch?> GetByCodeAsync(string code)
        {
            return await _context.Batches
                                 .FirstOrDefaultAsync(b => b.Code == code);
        }

        public async Task<bool> IsProductInUseAsync(Guid productId)
        {
            return await _context.Batches
                                 .AnyAsync(b => b.ProductId == productId && !b.isDeleted);
        }

        public async Task<bool> AreAllAssignmentsCompletedAsync(Guid batchId)
        {
            var count = await _context.Assignments.CountAsync(a => a.BatchId == batchId && a.Status == "Completed");
            return (count == 14);
        }

        public async Task<Batch?> GetByAssignmentIdAsync(Guid assignmentId)
        {
            return await _context.Batches.Include(b => b.Assignments).FirstOrDefaultAsync(b => b.Assignments.Any(a => a.Id == assignmentId));
        }

        public async Task<int?> GetLastCodeIndexAsync(string prefix)
        {
            var query = _context.Batches.Where(b => b.Code.StartsWith(prefix)).Select(b => b.Code.Substring(prefix.Length));
            var numberQuery = query.Select(b => int.Parse(b));

            if (!await numberQuery.AnyAsync())
            {
                return null;
            }

            var lastNumberString = await query
                .OrderByDescending(b => b.Length)
                .ThenByDescending(b => b)
                .FirstOrDefaultAsync();

            if (int.TryParse(lastNumberString, out var lastIndex))
            {
                return lastIndex;
            }

            return null;
        }

        public async Task<Batch?> GetByIdAssignmentWithMaterialUse(Guid assignmentId)
        {
            return await _context.Batches.Include(b => b.Assignments).Include(batch => batch.MaterialUses).FirstOrDefaultAsync(b => b.Assignments.Any(a => a.Id == assignmentId));
        }
    }
}
