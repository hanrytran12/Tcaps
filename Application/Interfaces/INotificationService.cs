namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode);

        Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest);
    }
}
