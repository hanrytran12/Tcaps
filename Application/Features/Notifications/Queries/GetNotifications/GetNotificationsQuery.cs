using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Notifications.Queries.GetNotifications
{
    public class GetNotificationsQuery : IRequest<List<NotificationDTO>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string? Type { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool? UnreadOnly { get; set; }
    }
}
