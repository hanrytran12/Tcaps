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
        Task<IEnumerable<Batch>> SearchAsync(Guid productId, DateOnly TargetDate);
        Task<Batch?> GetByIdWithAssignmentsAsync(Guid Id);
        Task<Batch> GetAggregateRootByAssignmentIdAsync(Guid assignmentId);
        Task<Batch?> GetByCodeAsync(string code);
        Task<bool> IsProductInUseAsync(Guid productId);
    }
}
