using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Productions.Events
{
    public class ProductionReportedEventHandler : INotificationHandler<ProductionReportedEvent>
    {
        private readonly INotificationService _notificationService;

        public ProductionReportedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ProductionReportedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendProductionReportNotificationAsync(
                notification.AssignId,
                notification.StaffId,
                notification.Quantity);
        }
    }
}
