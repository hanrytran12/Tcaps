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
    public class AddMaterialSupplyForQcWorkshopEventHandler : INotificationHandler<AddMaterialSupplyForQcWorkshopEvent>
    {
        private readonly INotificationService _notification;

        public AddMaterialSupplyForQcWorkshopEventHandler(INotificationService notification)
        {
            _notification = notification;
        }
        public async Task Handle(AddMaterialSupplyForQcWorkshopEvent notification, CancellationToken cancellationToken)
        {
            await _notification.SendAddMaterialSupplyForQcWorkshopNotification(notification.QcId, notification.RequestId, notification.MaterialId, notification.Quantity);
        }
    }
}
