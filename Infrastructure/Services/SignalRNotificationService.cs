using API.Hubs;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class SignalRNotificationService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationToGroupAsync(string groupName, string eventName, object payload)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(eventName, payload);
        }
    }
}
