using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class TransferRequestAddedEventHandler : INotificationHandler<TransferRequestAddedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;

        public TransferRequestAddedEventHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IWorkshopRepository workshopRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;
        }

        public async Task Handle(TransferRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            var userQc = await _userRepository.GetByIdAsync(notification.UserId);
            var workshop = await _workshopRepository.GetByIdAsync(userQc.WorkshopId);
            var user = await _userRepository.GetByRoleAsync("Lead");
            var title = "Yêu cầu duyệt chuyển giao mới";
            var message = $"QC {userQc.FullName} ở xưởng {workshop.Name} vừa gửi một yêu cầu duyệt công đoạn. Vui lòng kiểm tra.";
            var type = "TRANSFER_REQUEST";

            var noti = Notification.Create(user.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);

        }
    }
}
