using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IAssignmentCompletionService
    {
        Task<SummaryCalculateCompletedDTO> CalculateCompletedQuantityAsync(Guid assignmentId, Guid? reworkRequestId);
    }
}
