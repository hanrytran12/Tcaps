using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssignmentTransferRequest.Events
{
    public class NotifyNextQcOnReworkRequestCompletedEvent : INotificationHandler<ReworkRequestCompletedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly INotificationRepository _notificationRepository;

        public NotifyNextQcOnReworkRequestCompletedEvent(IAppDbContext appDbContext, INotificationRepository notificationRepository)
        {
            _appDbContext = appDbContext;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(ReworkRequestCompletedEvent notification, CancellationToken cancellationToken)
        {
            var query = from r in _appDbContext.ReworkRequests
                        where r.Id == notification.ReworkRequestId
                        join a in _appDbContext.Assignments on r.AssignmentId equals a.Id
                        select new { r, a };

            var queryInfo = await query.FirstOrDefaultAsync();

            var assigment = await _appDbContext.Assignments.Where(a => a.BatchId == queryInfo.a.BatchId).ToListAsync();
            var stepCurrent = queryInfo.a.StepOrder;
            var nextAssigment = assigment.Where(a => a.StepOrder > stepCurrent).OrderBy(a => a.StepOrder).FirstOrDefault();

            var qcUser = await _appDbContext.Users.Where(u => u.WorkshopId == nextAssigment.WorkshopId).FirstOrDefaultAsync();
            var workshopName = await _appDbContext.Workshops.Where(w => w.Id == queryInfo.a.WorkshopId).Select(w => w.Name).FirstOrDefaultAsync(cancellationToken);
            var batchCode = await _appDbContext.Batches.Where(b => b.Id == queryInfo.a.BatchId).Select(b => b.Code).FirstOrDefaultAsync();

            var type = "REWORK_DELIVERY_INCOMING";
            var title = "Lô hàng làm lại được chuyển giao";
            var message = $"Bạn sẽ nhận thêm {(int)queryInfo.r.DefectiveQuantity} sản phẩm làm lại cho lô {batchCode}. Lô sản phẩm từ xưởng {workshopName} sẽ giao tới vào ngày {queryInfo.r.NextStepDeliveryDate.Value.ToString("dd/MM/yyyy")}";

            var noti = Notification.Create(qcUser.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
