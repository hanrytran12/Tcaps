using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Events
{
    public class AssignmentAddedEventHandler : INotificationHandler<AssignmentAddedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRealtimeService _notificationService;

        public AssignmentAddedEventHandler(IWorkshopRepository workshopRepository, INotificationRepository notificationRepository, IUserRepository userRepository, INotificationRealtimeService notificationRealtimeService)
        {
            _workshopRepository = workshopRepository;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _notificationService = notificationRealtimeService;
        }

        public async Task Handle(AssignmentAddedEvent notificationEvent, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetQCByWorkshopIdAsync(notificationEvent.WorkshopId);
            if (user is null) return;

            var workshop = await _workshopRepository.GetByIdAsync(notificationEvent.WorkshopId);

            var title = "Kế hoạch sản xuất mới";
            var message = string.Empty;
            var type = "NEW_ASSIGNMENT";

            if (notificationEvent.ExpectedDeliveryDate.HasValue)
            {
                string formattedDate = notificationEvent.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy");

                message = $"Xưởng của bạn ({workshop?.Name}) vừa được phân công cho lô hàng {notificationEvent.BatchCode}. " +
                          $"Dự kiến nguyên vật liệu sẽ được giao vào ngày {formattedDate}.";
            }
            else
            {
                message = $"Xưởng của bạn ({workshop?.Name}) vừa được phân công cho lô hàng {notificationEvent.BatchCode}. " +
                          $"Vui lòng kiểm tra kế hoạch và chuẩn bị nhân lực.";
            }

            var notification = Notification.Create(user.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);

            await _notificationService.SendNotificationToGroupAsync(user.Id.ToString(), "ReceiveNotification", new
            {
                notification.Id,
                notification.Title,
                notification.Type,
                notification.Message,
                notification.CreatedAt,
                notification.IsRead
            });
        }
    }
}
