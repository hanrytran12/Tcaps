using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssignmentTransferRequest.Events
{
    public class AssignmentActivedEventHandler : INotificationHandler<AssignmentActivedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IUserRepository _userRepository;

        public AssignmentActivedEventHandler(INotificationRepository notificationRepository, IWorkshopRepository workshopRepository, IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _workshopRepository = workshopRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(AssignmentActivedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetQCByWorkshopIdAsync(notification.WorkshopIdNext);
            var workshop = await _workshopRepository.GetByIdAsync(notification.WorkshopId);

            var type = "ASSIGNMENT_ACTIVED";
            var meesage = $"Xưởng {workshop.Name} của lô hàng {notification.CodeBatch} đã làm xong lô sản phẩm và sẽ chuyển tới xưởng của bạn trước hoặc ngay ngày {notification.NextStartDate.ToString("dd/MM/yyyy")}.";
            var title = "Lô hàng được chuyển giao";

            var noti = Notification.Create(user.Id, title, meesage, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
