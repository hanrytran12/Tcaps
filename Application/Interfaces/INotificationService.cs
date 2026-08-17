using Application.DTOs.Response;
using Domain.Events;

using Application.Common;

namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid qcId, Guid batchId, decimal quantityRequest);
        Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task SendStockUpdateNotificationToAdminAsync(string name, int newStockQuantity, int stockChange);
        Task SendStockUpdateNotificationToLeadAsync(string name, int newStockQuantity, int stockChange);
        Task SendAssignmentAddNotificationToQcAsync(string batchCode, Guid workshopId, DateOnly expectedDeliveryDate);
        Task<ResponseDTO> MarkAsReadAsync(Guid notificationId);
        Task<Result<int>> CountNotificationAsync(Guid userId);
        Task SendEvaluateFixErrorNotificationAsync(Guid evaluateId, Guid productionId, Guid userId, int quantityError, int quantitySucess, string note, string status);
        Task SendSubmitProductionNotification(Guid assignId, Guid userId, int quantity);
        Task SendComponentResolvedNotification(Guid componentId, Guid evaluateId, int quantity, string status);
        Task SendComponentConfirmNotification(Guid componentId, Guid evaluateId, int quantity, string status);
        Task CreateStockUpdateNotificationForRoleAsync(Guid userId, string materialName, int newStock, int change);
        Task SendMaterialWorkshopConfirmNotificationAsync(Guid workshopId, int quantitySend, int quantityReceive, Guid? userId, string batchCode);
        Task SendCreateTaskTransferRequestNotificationAsync(Guid batchId, Guid workshopId, Guid qcTransportId, string note);
        Task SendApproveTaskTransferRequestNotificationAsync(Guid taskTransferRequestId, Guid qcTransportId);
        Task SendCreateMaterialRequestNotificationAsync(Guid qcId, Guid batchId, Guid assignId);
        Task AdminAssignForQCTransportToTransferMaterialSupplyNotificationAsync(Guid qcTransportId, Guid requestId, Guid materialId, int quantity);
        Task SendAddMaterialSupplyForQcWorkshopNotification(Guid qcworkshopId, Guid requestId, Guid materialId, int quantity);
        Task SendCompletedMaterialSupplyNotificationAsync(Guid userId, Guid materialId, string batchCode, int quantity);
        Task SendQCTransportApproveMaterialSupplyNotificationAsync(Guid qcTransportId, Guid materialSupplyId);
        Task SendQCTransportReceptionAssignmentTransferNotificationAsync(Guid qcTransportId, Guid assignTransferRequestId, Guid assignId);
        Task SendQCTransportReceptionMaterialRequestNotificationAsync(Guid qcTransportId, Guid materialRequestId, Guid assignId);
        Task SendConfirmRequestFromLeadNotificationAsync(Guid materialRequestId, Guid qcId);
        Task AddBatchNotificationAsync(Guid? userId, string batchCode, decimal quantity);
        Task ApproveFinalTransferRequestNotificationAsync(Guid batchId, decimal quantityComplete, decimal quantityError);
        Task UpdateQuantityDefectNotificationAsync(decimal quantityReject, Guid qcId, string batchCode);
        Task AssignWorkshopNotificationAsync(Guid userId, string batchCode);
        Task SendAddBatchForAdminNotificationAsync(string batchCode, decimal quantity);
        Task SendUpdateQuantityProductionNotificationAsync(Guid userId, decimal quantitySend, decimal quantityReceive, DateOnly date, TimeOnly time, string batchCode);
        Task SendFinalTransferRequestForGuardQCNotificationAsync(decimal quantitySend, string batchCode, string workshopName);
        Task SendProductionReportNotificationAsync(Guid assignId, Guid staffId, decimal quantity);
        Task SendAddMaterialSupplyForAdminNotification(Guid materialId, decimal quantitySend, DateTime dateShip);
        Task SendNotificationForStaffNotificationAsync(Guid userId, Guid batchId, decimal? quantity);
        Task SendAssignmentsPlannedNotificationAsync(List<AssignmentsInfo> assignments, string batchCode);
        Task SendTransferRequestNotificationAsync(Guid userId, Guid assignmentId);
        Task NotifyAdminDashboardRefreshAsync(Guid requestId);
        Task SendIncomingMaterialNotificationAsync(decimal quantity, Guid assignmentId, string materialName, string unitMaterial);
        Task BroadcastContributionUpdateAsync(Guid assignId, Guid staffId, decimal quantity);
        Task QCOnReworkRequestApproveNotificationAsync(Guid qcId, DateOnly deliveryDate, DateOnly endDate);
        Task LeadOnReworkRequestAddedNotificationAsync(Guid assignmentId, Guid qcId, decimal defectiveQuantity, string noteQC);
        Task CreateMaterialWorkshopNotificationAsync(Guid workshopPreviousId, Guid workshopAfterId, decimal quantitySend, string batchCode);
        Task RejectComponentDefectNotificationAsync(Guid componentId, Guid evaluateId, int quantity, int quantityReject);
        Task SendComponentRejectResolvedNotificationAsync(Guid componentId, Guid evaluateId, int quantity, int quantityReject);
        Task SendMaterialShortageNotificationAsync(Guid assignId, Guid staffId, Guid materialId, string materialName, string materialUnit, decimal quantityRemaining, Guid workshopId);
    }
}
