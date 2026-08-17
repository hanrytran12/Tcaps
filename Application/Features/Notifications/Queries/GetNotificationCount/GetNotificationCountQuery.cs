using Application.Common;
using MediatR;

namespace Application.Features.Notifications.Queries.GetNotificationCount
{
    public record GetNotificationCountQuery(Guid UserId) : IRequest<Result<int>>;
}
