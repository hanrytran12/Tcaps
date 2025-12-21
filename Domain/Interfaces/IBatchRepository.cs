using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetAllAsync();
        Task<Batch?> GetByIdAsync(Guid Id);
        Task<IEnumerable<Batch>> GetBatchesByIdsAsync(List<Guid> ids);
        Task AddAsync(Batch batch);
        void Delete(Batch batch);
        void Update(Batch batch);
        Task<Batch?> GetByIdWithAssignmentsAsync(Guid Id);
        Task<Batch?> GetByCodeAsync(string code);
        Task<bool> IsProductInUseAsync(Guid productId);
        Task<bool> AreAllAssignmentsCompletedAsync(Guid batchId);
        Task<Batch?> GetByAssignmentIdAsync(Guid assignmentId);
        Task<int?> GetLastCodeIndexAsync(string prefix);
        Task<Batch?> GetByIdAssignmentWithMaterialUse(Guid assignmentId);
        Task<List<Batch>> GetBatchesByLeadIdAsync(Guid userId);
    }
}
