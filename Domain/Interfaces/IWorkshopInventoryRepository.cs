using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkshopInventoryRepository
    {
        Task AddAsync(WorkshopInventory workshopInventory);
    }
}
