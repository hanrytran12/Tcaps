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
            return await _context.Batches.ToListAsync();
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

        public async Task<Batch> GetAggregateRootByAssignmentIdAsync(Guid assignmentId)
        {
            return await _context.Batches.Include(b => b.Assignments)
                                 .FirstOrDefaultAsync(b => b.Assignments.Any(a => a.Id == assignmentId));
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
    }
}
