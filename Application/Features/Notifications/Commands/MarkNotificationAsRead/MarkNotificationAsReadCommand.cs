using Application.Common;
using MediatR;

namespace Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<Result>
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
