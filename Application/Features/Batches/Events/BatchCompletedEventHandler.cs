using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Events
{
    public class BatchCompletedEventHandler : INotificationHandler<BatchCompletedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppDbContext _appDbContext;

        public BatchCompletedEventHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IAppDbContext appDbContext)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }

        public async Task Handle(BatchCompletedEvent notification, CancellationToken cancellationToken)
        {
            var query = from b in _appDbContext.Batches
                        where b.Id == notification.BatchId
                        join p in _appDbContext.Products on b.ProductId equals p.Id
                        select new { p.Name };

            var queryInfo = await query.FirstOrDefaultAsync();

            var user = await _userRepository.GetByRoleAsync("Admin");
            var title = $"Lô hàng {notification.BatchCode} đã hoàn thành";
            var message = $"Lô hàng {notification.BatchCode} của {queryInfo?.Name} đã hoàn thành toàn bộ công đoạn.";
            var type = "BATCH_COMPLETION";

            var result = Notification.Create(user.Id, title, message, type);

            await _notificationRepository.AddAsync(result);
        }
    }
}
