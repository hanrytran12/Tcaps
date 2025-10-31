using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Material material)
        {
            await _context.Materials.AddAsync(material);
        }

        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            return await _context.Materials.ToListAsync();
        }

        public async Task<Material?> GetByIdAsync(Guid materialId)
        {
            return await _context.Materials.FindAsync(materialId);
        }

        public async Task<Material?> GetByNameAsync(string name)
        {
            return await _context.Materials.FirstOrDefaultAsync(i => i.Name == name);
        }
    }
}
