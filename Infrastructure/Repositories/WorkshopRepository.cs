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
            await _context.Workshops.AddAsync(workshop);
        }

        public void Delete(Workshop workshop)
        {
            _context.Workshops.Remove(workshop);
        }

        public async Task<bool> ExistNameAsync(string name)
        {
            return await _context.Workshops.AnyAsync(ws => ws.Name == name);
        }

        public Task<bool> ExistsAsync(Guid? id)
        {
            return _context.Workshops.AnyAsync(ws => ws.Id == id);
        }

        public async Task<bool> ExistsStepOrderAsync(int stepOrder)
        {
            return await _context.Workshops.AnyAsync(ws => ws.StepOrder == stepOrder);
        }

        public async Task<IEnumerable<Workshop>> FindByNameAsync(string name)
        {
            return await _context.Workshops.Where(ws => ws.Name.Contains(name)).ToListAsync();
        }

        public async Task<IEnumerable<Workshop>> GetAllAsync()
        {
            return await _context.Workshops.ToListAsync();
        }

        public async Task<Workshop?> GetByIdAsync(Guid? id)
        {
            return await _context.Workshops.FindAsync(id);
        }

        public async Task<List<Workshop>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Workshops
                .AsNoTracking()
                .Where(w => ids.Contains(w.Id))
                .ToListAsync();
        }

        public async Task<int?> GetMaxStepOrderAsync()
        {
            return await _context.Workshops
                .Where(w => w.WorkshopType == Domain.Enums.WorkshopType.Internal)
                .MaxAsync(w => w.StepOrder);
        }

        public async Task ShiftStepOrdersUpAsync(int from, int to)
        {
            await _context.Workshops
                .Where(w => w.StepOrder >= from && w.StepOrder <= to)
                .ExecuteUpdateAsync(s => s.SetProperty(w => w.StepOrder, w => w.StepOrder - 1));
        }

        public async Task ShiftStepOrdersDownAsync(int from, int to)
        {
            await _context.Workshops
                .Where(w => w.StepOrder >= from && w.StepOrder <= to)
                .ExecuteUpdateAsync(s => s.SetProperty(w => w.StepOrder, w => w.StepOrder + 1));
        }

        public async Task ReindexFromAsync(int fromStepOrder)
        {
            var workshops = await _context.Workshops
                .Where(w => w.StepOrder != null && w.StepOrder >= fromStepOrder)
                .OrderBy(w => w.StepOrder)
                .ToListAsync();

            foreach (var w in workshops)
            {
                w.DecreaseStepOrder();
            }
        }


        public void Update(Workshop workshop)
        {
            _context.Workshops.Update(workshop);
        }
    }
}
