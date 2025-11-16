using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Events
{
    public class NotifyQcOnReworkRequestApprovedHandler : INotificationHandler<ReworkRequestApprovedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly INotificationRepository _notificationRepository;

        public NotifyQcOnReworkRequestApprovedHandler(IAppDbContext appDbContext, INotificationRepository notificationRepository)
        {
            _appDbContext = appDbContext;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(ReworkRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            var qcUser = await _appDbContext.Users.AsNoTracking().Where(u => u.Id == notification.QcId).FirstOrDefaultAsync(cancellationToken);

            var type = "REWORK_REQUEST_APPROVED";
            var title = "Yêu cầu làm lại sản phẩm lỗi";
            var message = $"Xưởng bạn sẽ làm lại sản phẩm lỗi. Nhận nguyên vật liệu vào ngày {notification.DeliveryDate.ToString("dd/MM/yyyy")} và hoàn thành trước ngày {notification.EndDate.ToString("dd/MM/yyyy")}";

            var noti = Notification.Create(qcUser.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
