using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class TransferRequestAddedEventHandler : INotificationHandler<TransferRequestAddedEvent>
    {
        private readonly INotificationService _notificationService;

        public TransferRequestAddedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(TransferRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendTransferRequestNotificationAsync(notification.UserId, notification.AssignmentId);
        }
    }
}
