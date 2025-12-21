using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkshopRepository
    {
        Task<Workshop?> GetByIdAsync(Guid? id);
        Task<IEnumerable<Workshop>> GetAllAsync();
        Task<IEnumerable<Workshop>> FindByNameAsync(string name);
        Task AddAsync(Workshop workshop);
        void Update(Workshop workshop);
        void Delete(Workshop workshop);
        Task<bool> ExistsAsync(Guid? id);
        Task<List<Workshop>> GetByIdsAsync(List<Guid> ids);
        Task<bool> ExistNameAsync(string name);
        Task<bool> ExistsStepOrderAsync(int stepOrder);
        Task<int?> GetMaxStepOrderAsync();
        Task ShiftStepOrdersUpAsync(int from, int to);
        Task ShiftStepOrdersDownAsync(int from, int to);
        Task ReindexFromAsync(int fromStepOrder);
    }
}
