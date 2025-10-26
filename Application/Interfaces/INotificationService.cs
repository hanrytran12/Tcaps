using Application.DTOs;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode);
        Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest);
        Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task SendStockUpdateNotificationToAdminAsync(string name, int newStockQuantity, int stockChange);
        Task SendAssignmentAddNotificationToQcAsync(string batchCode, Guid workshopId);
        Task<ResponseDTO> MarkAsReadAsync(Guid notificationId);
        Task<ResponseDTO> CountNotificationAsync(Guid userId);
    }
}
