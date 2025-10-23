using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
            if (notification is null)
            {
                return Result.Failure("Notification not found.");
            }
            if (notification.UserId != request.UserId)
            {
                return Result.Failure("You not have permission to do that.");
            }

            notification.MarkAsRead();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
