using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkshopRepository : IWorkshopRepository
    {
        private readonly AppDbContext _context;

        public WorkshopRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Workshop workshop)
        {
            await _context.Workshop.AddAsync(workshop);
        }

        public void Delete(Workshop workshop)
        {
            _context.Workshop.Remove(workshop);
        }

        public Task<bool> ExistsAsync(Guid? id)
        {
            return _context.Workshop.AnyAsync(ws => ws.Id == id);
        }

        public async Task<IEnumerable<Workshop>> FindByNameAsync(string name)
        {
            return await _context.Workshop.Where(ws => ws.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Workshop>> GetAllAsync()
        {
            return await _context.Workshop.ToListAsync();
        }

        public async Task<Workshop?> GetByIdAsync(Guid? id)
        {
            return await _context.Workshop.FindAsync(id);
        }

        public async Task<List<Workshop>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Workshop
                .AsNoTracking()
                .Where(w => ids.Contains(w.Id))
                .ToListAsync();
        }

        public void Update(Workshop workshop)
        {
            _context.Workshop.Update(workshop);
        }
    }
}
