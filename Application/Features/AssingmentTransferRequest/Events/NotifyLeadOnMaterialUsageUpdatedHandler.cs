using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using System.Text;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class NotifyLeadOnMaterialUsageUpdatedHandler : INotificationHandler<MaterialUsageReconciledEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMaterialUseRepository _materialUseRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotifyLeadOnMaterialUsageUpdatedHandler(IUserRepository userRepository, IMaterialUseRepository materialUseRepository, IMaterialRepository materialRepository, IWorkshopRepository workshopRepository, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _materialUseRepository = materialUseRepository;
            _materialRepository = materialRepository;
            _workshopRepository = workshopRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(MaterialUsageReconciledEvent notification, CancellationToken cancellationToken)
        {
            var leadUser = await _userRepository.GetByRoleAsync("Lead");
            var qcUser = await _userRepository.GetByIdAsync(notification.UserId);
            var materialUse = await _materialUseRepository.GetByIdAsync(notification.MaterialUseId);
            var material = await _materialRepository.GetByIdAsync(materialUse.MaterialId);
            var workshop = await _workshopRepository.GetByIdAsync(qcUser.WorkshopId);

            var type = "MATERIAL_RECONCILED";
            var title = "Cập nhật nguyên vật liệu";
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"QC {qcUser.FullName} vừa chốt sổ vật tư cho Lô {notification.BatchCode} tại xưởng {workshop.Name}.");
            messageBuilder.AppendLine($"Vật tư: {material.Name}");
            messageBuilder.AppendLine($"Đã cấp: {materialUse.QuantityDivide}");
            messageBuilder.AppendLine($"Staff báo cáo: {materialUse.QuantityStaffUse}");
            messageBuilder.AppendLine($"QC chốt sổ: {notification.ReconciledQuantity}");
            //messageBuilder.Append($"Ghi chú: {notificationEvent.Note}");
            var message = messageBuilder.ToString();

            var noti = Notification.Create(leadUser.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
