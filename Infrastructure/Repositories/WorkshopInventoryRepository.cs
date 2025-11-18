using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkshopInventoryRepository : IWorkshopInventoryRepository
    {
        private readonly AppDbContext _appDbContext;
        public WorkshopInventoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(WorkshopInventory workshopInventory)
        {
            await _appDbContext.WorkshopInventory.AddAsync(workshopInventory);
        }


        public async Task<WorkshopInventory?> GetByMaterialIdAndWorkshopIdAsync(Guid materialId, Guid workshopId)
        {
            return await _appDbContext.WorkshopInventory.FirstOrDefaultAsync(x => x.MaterialId == materialId && x.WorkshopId == workshopId);

        }

        public async Task<WorkshopInventory> GetByMaterialIdAsync(Guid materialId)
        {
            return await _appDbContext.WorkshopInventory
                .FirstOrDefaultAsync(x => x.MaterialId == materialId);
        }
    }
}
