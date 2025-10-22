namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode);
    }
}
