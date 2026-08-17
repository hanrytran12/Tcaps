using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            var workshopId = Context.User?.FindFirst("WorkshopId")?.Value;

            if (!string.IsNullOrEmpty(workshopId))
            {
                var groupName = $"Workshop_{workshopId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            if (role == "Admin")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}