using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Events
{
    public class NotifyQcOnMaterialRequestAddedEvent : INotificationHandler<MaterialRequestAddedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly INotificationRepository _notificationRepository;

        public NotifyQcOnMaterialRequestAddedEvent(IAppDbContext appDbContext, INotificationRepository notificationRepository)
        {
            _appDbContext = appDbContext;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(MaterialRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            var query = from a in _appDbContext.Assignments
                        where a.Id == notification.AssignmentId
                        join u in _appDbContext.Users on a.WorkshopId equals u.WorkshopId
                        where u.Role == "QC"
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        select new { a.ExpectedDeliveryDate, b.Code, u.Id };

            var queryInfo = await query.FirstOrDefaultAsync();

            var type = "MATERIAL_DELIVERY_INCOMING";
            var title = "Thông báo nhận nguyên vật liệu";
            var message = $"Nguyên vật liệu {notification.MaterialName} với số lượng {notification.Quantity} {notification.UnitMaterial} sẽ được giao tới xưởng bạn vào ngày {queryInfo.ExpectedDeliveryDate} để làm sản phẩm cho lô hàng {queryInfo.Code}.";

            var noti = Notification.Create(queryInfo.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
