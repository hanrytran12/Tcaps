using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Events
{
    public class NotifyLeadOnReworkRequestAddedHandler : INotificationHandler<ReworkRequestAddedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly INotificationRepository _notificationRepository;

        public NotifyLeadOnReworkRequestAddedHandler(IAppDbContext appDbContext, INotificationRepository notificationRepository)
        {
            _appDbContext = appDbContext;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(ReworkRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            var leadUser = await _appDbContext.Users.Where(u => u.Role == "Lead").FirstOrDefaultAsync(cancellationToken);
            var qcUser = await _appDbContext.Users.Where(u => u.Id == notification.QcId).FirstOrDefaultAsync(cancellationToken);

            var query = from a in _appDbContext.Assignments
                        where a.Id == notification.AssignmentId
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                        select new { batchCode = b.Code, workshopName = w.Name };

            var assignmentInfo = await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            var batchCode = assignmentInfo.batchCode;
            var workshopName = assignmentInfo.workshopName;

            var type = "REWORK_REQUEST_CREATED";
            var title = "Yêu cầu thông báo có sản phẩm lỗi khi chuyển giao";
            var body = $"Lô hàng thuộc lô {batchCode} ở xưởng {workshopName} của QC {qcUser.FullName} hiện tại đang có {notification.DefectiveQuantity} sản phẩm lỗi, ghi chú từ QC: {notification.NoteQC}";

            var noti = Notification.Create(leadUser.Id, title, body, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
