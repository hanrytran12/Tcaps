using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssignmentTransferRequestRepository
    {
        Task AddAsync(AssignmentTransferRequest assignmentTransferRequest);
    }
}
