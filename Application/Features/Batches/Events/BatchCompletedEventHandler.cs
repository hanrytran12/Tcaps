using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Batches.Events
{
    public class BatchCompletedEventHandler : INotificationHandler<BatchCompletedEvent>
    {
        private readonly INotificationService _notificationService;
        public BatchCompletedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(BatchCompletedEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendBatchCompletionNotificationAsync(notification.BatchId, notification.BatchCode);
        }
    }
}
