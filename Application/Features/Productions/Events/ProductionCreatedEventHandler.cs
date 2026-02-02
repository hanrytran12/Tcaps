using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Productions.Events
{
    public class ProductionCreatedEventHandler : INotificationHandler<ProductionCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public ProductionCreatedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(ProductionCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Event handler chỉ gửi notification, không xử lý MaterialUse
            // MaterialUse được xử lý trong AddProductionReportCommandHandler với thông tin chi tiết hơn
            await _notificationService.SendSubmitProductionNotification(notification.AssignId, notification.StaffId, notification.Quantity);
        }
    }
}
