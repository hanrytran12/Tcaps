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
    public class CreateMaterialWorkshopEventHandler : INotificationHandler<CreateMaterialWorkshopEvent>
    {
        private readonly INotificationService _notificationService;

        public CreateMaterialWorkshopEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(CreateMaterialWorkshopEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.CreateMaterialWorkshopNotificationAsync(
                notification.WorkshopPreviousId,
                notification.WorkshopAfterId,
                notification.QuantitySend,
                notification.BatchCode);
        }
    }
}
