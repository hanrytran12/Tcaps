using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

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
    }
}
