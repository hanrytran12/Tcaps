using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<Material?> GetByIdAsync(Guid materialId);
        Task<IEnumerable<Material>> GetAllAsync();
        Task AddAsync(Material material);
        Task<Material?> GetByNameAsync(string name);
    }
}
