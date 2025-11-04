using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialWorkshops.Events
{
    public class MaterialWorkshopConfirmEventHandler : INotificationHandler<MaterialWorkshopConfirmEvent>
    {
        private readonly INotificationService _notificationService;

        public MaterialWorkshopConfirmEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(MaterialWorkshopConfirmEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMaterialWorkshopConfirmNotificationAsync(
                notification.WorkshopId,
                notification.QuantitySend,
                notification.QuantityReceive,
                notification.Name);
        }
    }
}
