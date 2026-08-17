using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Notifications.Queries.GetNotificationCount
{
    public class GetNotificationCountQueryHandler : IRequestHandler<GetNotificationCountQuery, Result<int>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public GetNotificationCountQueryHandler(
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<int>> Handle(GetNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<int>.NotFound("User not found.", "user_not_found");
            }

            var count = await _notificationRepository.CountNotificationAsync(request.UserId);
            return Result<int>.Success(count);
        }
    }
}
