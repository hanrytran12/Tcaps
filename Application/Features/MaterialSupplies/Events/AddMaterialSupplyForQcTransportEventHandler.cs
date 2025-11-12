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
    public class AddMaterialSupplyForQcTransportEventHandler : INotificationHandler<AddMaterialSupplyForQcTransportEvent>
    {
        private readonly INotificationService _notificationService;

        public AddMaterialSupplyForQcTransportEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(AddMaterialSupplyForQcTransportEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendAddMaterialSupplyNotificationAsync(notification.QcId, notification.RequestId, notification.MaterialId, notification.Quantity);
        }
    }
}
