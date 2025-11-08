using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TaskTransferRequests.Events
{
    public class CreateTaskTransferRequestEventHandler : INotificationHandler<CreateTaskTransferRequestEvent>
    {
        private readonly INotificationService _notificationService;

        public CreateTaskTransferRequestEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task Handle(CreateTaskTransferRequestEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendCreateTaskTransferRequestNotificationAsync(
                notification.BatchId,
                notification.WorkshopId,
                notification.QcTransportId,
                notification.Note);
        }
    }
}
