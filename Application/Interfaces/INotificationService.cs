using Application.DTOs;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest);
        Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task SendStockUpdateNotificationToAdminAsync(string name, int newStockQuantity, int stockChange);
        Task SendStockUpdateNotificationToLeadAsync(string name, int newStockQuantity, int stockChange);
        Task SendAssignmentAddNotificationToQcAsync(string batchCode, Guid workshopId, DateOnly expectedDeliveryDate);
        Task<ResponseDTO> MarkAsReadAsync(Guid notificationId);
        Task<ResponseDTO> CountNotificationAsync(Guid userId);
        Task SendEvaluateFixErrorNotificationAsync(Guid evaluateId, Guid productionId, Guid userId, int quantityError, string note, string status);
        Task SendSubmitProductionNotification(Guid assignId, Guid userId, int quantity);
        Task SendComponentResolvedNotification(Guid componentId, Guid evaluateId, int quantity, string status);
        Task SendComponentConfirmNotification(Guid componentId, Guid evaluateId, int quantity, string status);
        Task CreateStockUpdateNotificationForRoleAsync(string role, string materialName, int newStock, int change);
        Task SendMaterialWorkshopConfirmNotificationAsync(Guid workshopId, int quantitySend, int quantityReceive, string name);
    }
}
