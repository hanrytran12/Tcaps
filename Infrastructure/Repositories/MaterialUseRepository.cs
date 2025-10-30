using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class MaterialUseRepository : IMaterialUseRepository
    {
        private readonly AppDbContext _context;
        public MaterialUseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaterialUse materialUse)
        {
            await _context.MaterialUses.AddAsync(materialUse);
        }
    }
}
