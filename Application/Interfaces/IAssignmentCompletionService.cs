namespace Application.Interfaces
{
    public interface IAssignmentCompletionService
    {
        Task<decimal> CalculateCompetedQuantityAsync(Guid assignmentId);
    }
}
