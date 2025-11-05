using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetAssignmentsByWorkshopIdAsync(Guid workshopId);
        Task<Assignment> GetByIdAsync(Guid id);
        Task<IEnumerable<Assignment>> GetByIdsAsync(List<Guid> ids);
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
        Task AddAsync(Assignment assignment);
        void Update(Assignment assignment);
        Task<bool> ExistsAndBelongsToBatchAsync(Guid assignmentId, Guid batchId);
        Task<Assignment?> FindByBatchAndStepOrderAsync(Guid batchId, int stepOrder);
    }
}
