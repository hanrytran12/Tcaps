using Application.DTOs;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode);

        Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest);
        Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId);
        Task<ResponseDTO> MarkAsReadAsync(Guid notificationId);
    }
}
