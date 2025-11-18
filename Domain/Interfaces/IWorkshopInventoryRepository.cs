using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkshopInventoryRepository
    {
        Task<WorkshopInventory?> GetByMaterialIdAndWorkshopIdAsync(Guid materialId, Guid workshopId);
        Task AddAsync(WorkshopInventory workshopInventory);
        Task<WorkshopInventory> GetByMaterialIdAsync(Guid materialId);
    }
}
