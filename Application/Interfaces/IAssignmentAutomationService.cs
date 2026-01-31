namespace Application.Interfaces
{
    public interface IAssignmentAutomationService
    {
        Task ProcessDailyUpdatesAsync(CancellationToken cancellationToken);
    }
}
