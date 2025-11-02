using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssignmentTransferRequestRepository
    {
        Task<AssignmentTransferRequest?> GetByIdAsync(Guid transferRequestId);
        Task AddAsync(AssignmentTransferRequest assignmentTransferRequest);
    }
}
