using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.MaterialSupplies.Events
{
    public class CompletedMaterialSupplyEventHandler : INotificationHandler<CompletedMaterialSupplyEvent>
    {
        private readonly INotificationService _notificationService;

        public CompletedMaterialSupplyEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(CompletedMaterialSupplyEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendCompletedMaterialSupplyNotificationAsync(
                notification.UserId, 
                notification.MaterialId,
                notification.BatchCode,
                notification.Quantity);
        }
    }
}
