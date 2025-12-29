using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Events
{
    public class AssignmentsPlannedEventHandler : INotificationHandler<AssignmentsPlannedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignmentsPlannedEventHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IWorkshopRepository workshopRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AssignmentsPlannedEvent notification, CancellationToken cancellationToken)
        {
            var workshopIds = notification.Assignments.Select(a => a.WorkshopId).ToList();
            var workshops = await _workshopRepository.GetByIdsAsync(workshopIds);
            var qcUsers = await _userRepository.GetQcsByWorkshopIdsAsync(workshopIds);

            foreach (var assignmentInfo in notification.Assignments)
            {
                var workshop = workshops.FirstOrDefault(w => w.Id == assignmentInfo.WorkshopId);
                var qc = qcUsers.FirstOrDefault(u => u.WorkshopId == assignmentInfo.WorkshopId);

                if (qc is null || workshop is null) continue;

                var title = "Kế hoạch sản xuất mới";
                var message = string.Empty;
                var type = "NEW_ASSIGNMENT";

                if (assignmentInfo.ExpectedDeliveryDate.HasValue)
                {
                    string formattedDate = assignmentInfo.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy");

                    message = $"Xưởng của bạn ({workshop?.Name}) vừa được phân công cho lô hàng {notification.BatchCode}. " +
                              $"Dự kiến nguyên vật liệu sẽ được giao vào ngày {formattedDate}.";
                }
                else
                {
                    message = $"Xưởng của bạn ({workshop?.Name}) vừa được phân công cho lô hàng {notification.BatchCode}. " +
                              $"Vui lòng kiểm tra kế hoạch và chuẩn bị nhân lực.";
                }

                var noti = Notification.Create(qc.Id, title, message, type);
                await _notificationRepository.AddAsync(noti);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
