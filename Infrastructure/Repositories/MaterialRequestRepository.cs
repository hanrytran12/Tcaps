using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MaterialRequestRepository : IMaterialRequestRepository
    {
        private readonly AppDbContext _context;
        public MaterialRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaterialRequest materialRequest)
        {
            await _context.MaterialRequests.AddAsync(materialRequest);
        }

        public void Delete(MaterialRequest materialRequest)
        {
            _context.MaterialRequests.Remove(materialRequest);
        }

        public async Task<IEnumerable<MaterialRequest>> GetAllAsync()
        {
            return await _context.MaterialRequests.ToListAsync();
        }

        public async Task<MaterialRequest?> GetByIdAsync(Guid id)
        {
            return await _context.MaterialRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(MaterialRequest materialRequest)
        {
            _context.MaterialRequests.Update(materialRequest);
        }
    }
}
