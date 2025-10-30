using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Events
{
    public class BatchCompletedEventHandler : INotificationHandler<BatchCompletedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public BatchCompletedEventHandler(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(BatchCompletedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByRoleAsync("Admin");
            var title = $"Lô hàng {notification.BatchCode} đã hoàn thành";
            var message = $"Lô hàng {notification.BatchCode} với ID {notification.BatchId} đã hoàn thành toàn bộ công đoạn.";
            var type = "BatchCompletion";

            var result = Notification.Create(user.Id, title, message, type);

            await _notificationRepository.AddAsync(result);
        }
    }
}
