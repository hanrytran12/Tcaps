using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<Material?> GetByIdAsync(Guid materialId);
    }
}
