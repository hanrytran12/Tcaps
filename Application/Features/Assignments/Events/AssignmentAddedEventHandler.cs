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
            var workshop = await _workshopRepository.GetByIdAsync(notificationEvent.WorkshopId);

            var title = "Công việc mới được giao";
            var message = string.Empty;
            var type = "NEW_ASSIGNMENT";
            string formattedDate = (notificationEvent.ExpectedDeliveryDate is not null) ? notificationEvent.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy") : "";

            if (notificationEvent.ExpectedDeliveryDate != null)
            {
                message = $"Một lô hàng mới, mã lô {notificationEvent.BatchCode}, vừa được phân công cho xưởng của bạn ({workshop?.Name}). Dự kiến giao nguyên liệu vào ngày {formattedDate}";

            }
            else
            {
                message = $"Một lô hàng mới, mã lô {notificationEvent.BatchCode}, vừa được phân công cho xưởng của bạn ({workshop?.Name}).";
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
