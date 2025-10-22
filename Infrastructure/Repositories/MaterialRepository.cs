using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;
        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Material?> GetByIdAsync(Guid materialId)
        {
            return await _context.Materials.FindAsync(materialId);
        }
    }
}
