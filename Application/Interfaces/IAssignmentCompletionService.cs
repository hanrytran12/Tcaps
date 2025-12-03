using Application.DTOs.Response;

namespace Application.Interfaces
{
    public interface IAssignmentCompletionService
    {
        Task<SummaryCalculateCompletedDTO> CalculateCompetedQuantityAsync(Guid assignmentId, Guid? reworkRequestId);
    }
}
