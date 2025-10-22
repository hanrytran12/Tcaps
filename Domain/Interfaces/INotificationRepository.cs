using Domain.Entities;

namespace Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<Notification?> GetByIdAsync(Guid id);
        Task AddAsync(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
        Task MarkAsReadAsync(Guid notificationId);
    }
}
