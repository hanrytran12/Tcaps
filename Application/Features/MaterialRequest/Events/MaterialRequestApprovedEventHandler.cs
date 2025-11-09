using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestApprovedEventHandler : INotificationHandler<MaterialRequestApprovedEvent>
    {
        private readonly INotificationService _notificationService;
        public MaterialRequestApprovedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(MaterialRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMaterialRequestApprovalNotificationAsync(notification.MaterialId, notification.Qc_Id, notification.BatchId, notification.QuantityRequest);
        }
    }
}
