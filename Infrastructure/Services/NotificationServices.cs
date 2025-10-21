using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class NotificationServices : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
        }

        public async Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode)
        {
            var user = await _userRepository.GetByRoleAsync("Admin");
            var title = $"Batch {batchCode} Completed";
            var message = $"Batch {batchCode} with ID {batchId} has been completed.";
            var type = "BatchCompletion";
            var notification = new Notification(Guid.NewGuid(), user.Id, title, message, type);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
