using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class TransferRequestAddedEventHandler : INotificationHandler<TransferRequestAddedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IAppDbContext _context;

        public TransferRequestAddedEventHandler(INotificationRepository notificationRepository, IUserRepository userRepository, IWorkshopRepository workshopRepository, IAppDbContext context)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;
            _context = context;
        }

        public async Task Handle(TransferRequestAddedEvent notification, CancellationToken cancellationToken)
        {
            var userQc = await _userRepository.GetByIdAsync(notification.UserId);
            var workshop = await _workshopRepository.GetByIdAsync(userQc.WorkshopId);
            var user = await (from a in _context.Assignments
                              join b in _context.Batches on a.BatchId equals b.Id
                              join u in _context.Users on b.UserId equals u.Id
                              where a.Id == notification.AssignmentId
                              select u)
                              .FirstOrDefaultAsync();
            
            var title = "Yêu cầu duyệt chuyển giao mới";
            var message = $"QC {userQc.FullName} ở xưởng {workshop.Name} vừa gửi một yêu cầu duyệt công đoạn. Vui lòng kiểm tra.";
            var type = "TRANSFER_REQUEST";

            var noti = Notification.Create(user.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);
        }
    }
}
