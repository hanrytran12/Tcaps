using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialRequestRepository
    {
        Task<IEnumerable<MaterialRequest>> GetAllAsync();
        Task<MaterialRequest?> GetByIdAsync(Guid id);
        Task AddAsync(MaterialRequest materialRequest);
        void Update(MaterialRequest materialRequest);
        void Delete(MaterialRequest materialRequest);
    }
}
