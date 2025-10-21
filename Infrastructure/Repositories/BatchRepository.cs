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

        public async Task<IEnumerable<Batch>> SearchAsync(Guid productId, DateOnly TargetDate)
        {
            var query = _context.Batches
                                .AsNoTracking()
                                .Where(b =>
                                        b.ProductId == productId &&
                                        TargetDate >= b.StartDate &&
                                        TargetDate <= b.EndDate
        );

            return await query.ToListAsync();
        }

        public async Task<Batch?> GetByIdAsync(Guid Id)
        {
            return await _context.Batches.FirstOrDefaultAsync(b => b.Id == Id);
        }

        public void Update(Batch batch)
        {
            _context.Update(batch);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByIdsAsync(List<Guid> ids)
        {
            return await _context.Batches
                .AsNoTracking()
                .Where(b =>  ids.Contains(b.Id))
                .ToListAsync();
        }
    }
}
