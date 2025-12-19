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

        public async Task<bool> ExistNameAsync(string name)
        {
            return await _context.Workshop.AnyAsync(ws => ws.Name == name);
        }

        public Task<bool> ExistsAsync(Guid? id)
        {
            return _context.Workshop.AnyAsync(ws => ws.Id == id);
        }

        public async Task<bool> ExistsStepOrderAsync(int stepOrder)
        {
            return await _context.Workshop.AnyAsync(ws => ws.StepOrder == stepOrder);
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

        public async Task<int?> GetMaxStepOrderAsync()
        {
            return await _context.Workshop
                .Where(w => w.WorkshopType == Domain.Enums.WorkshopType.Internal)
                .MaxAsync(w => w.StepOrder);
        }

        public async Task ShiftStepOrdersAsync(int fromStepOrder)
        {
            var workshops = await _context.Workshop
                .Where(w => w.StepOrder >= fromStepOrder)
                .OrderByDescending(w => w.StepOrder)
                .ToListAsync();

            foreach (var w in workshops)
            {
                w.IncreaseStepOrder();
            }
        }

        public async Task ShiftStepOrdersDownAsync(int fromStepOrder)
        {
            var workshops = await _context.Workshop
                .Where(w => w.StepOrder > fromStepOrder)
                .OrderBy(w => w.StepOrder)
                .ToListAsync();

            foreach (var w in workshops)
            {
                w.DecreaseStepOrder();
            }
        }

        public void Update(Workshop workshop)
        {
            _context.Workshop.Update(workshop);
        }
    }
}
