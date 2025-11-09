using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class CreateMaterialRequestEventHandler : INotificationHandler<CreateMaterialRequestEvent>
    {
        private readonly INotificationService _notificationService;

        public CreateMaterialRequestEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(CreateMaterialRequestEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendCreateMaterialRequestNotificationAsync(
                notification.QC_Id,
                notification.BatchId,
                notification.AssignId);
        }
    }
}
