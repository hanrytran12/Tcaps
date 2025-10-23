using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory inventory);
        Task<Inventory?> GetByIdAsync(Guid id);
        Task<IEnumerable<Inventory>> GetAllAsync();
        void UpdateAsync(Inventory inventory);
        void DeleteAsync(Inventory inventory);
    }
}
