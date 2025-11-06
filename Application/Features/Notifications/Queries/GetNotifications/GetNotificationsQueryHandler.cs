using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDTO>>
    {
        private readonly IAppDbContext _context;
        public GetNotificationsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDTO>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == request.UserId);

            if (request.UnreadOnly == true)
            {
                query = query.Where(n => !n.IsRead);
            }

            if (!string.IsNullOrEmpty(request.Type))
            {
                query = query.Where(n => n.Type == request.Type);
            }

            var notifications = query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(n => new NotificationDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedAt
                })
                .AsNoTracking()
                .ToListAsync();

            return await notifications;
        }
    }
}
