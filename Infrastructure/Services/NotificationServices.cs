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
        private readonly IMaterialRepository _materialRepository;
        private readonly IBatchRepository _batchRepository;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _materialRepository = materialRepository;
            _batchRepository = batchRepository;
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

        public async Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest)
        {
            var user = await _userRepository.GetByRoleAsync("Lead");
            var batch = await _batchRepository.GetByIdAsync(batchId);
            var material = await _materialRepository.GetByIdAsync(materialId);
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var title = "Material Request Approved";
            var message = $"Lead {user?.FullName} is approve request for {quantityRequest} of material {material?.Name} for batch {batch?.Code}.";
            var type = "MaterialRequestApproval";
            var notification = new Notification(Guid.NewGuid(), admin.Id, title, message, type);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
