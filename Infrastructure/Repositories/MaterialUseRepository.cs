using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MaterialUseRepository : IMaterialUseRepository
    {
        private readonly AppDbContext _context;

        public MaterialUseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MaterialUse>> GetAllAsync()
        {
            return await _context.MaterialUses.ToListAsync();
        }

        public async Task<List<MaterialUse>> GetByAssignIdAsync(Guid assignId)
        {
            return await _context.MaterialUses.Where(m => m.AssignId == assignId).ToListAsync();
        }

        public async Task<MaterialUse> GetByIdAsync(Guid id)
        {
            return await _context.MaterialUses.FindAsync(id);
        }

        public void Update(MaterialUse materialUse)
        {
            _context.MaterialUses.Update(materialUse);
        }

        public async Task AddAsync(MaterialUse materialUse)
        {
            await _context.MaterialUses.AddAsync(materialUse);
        }
    }
}
