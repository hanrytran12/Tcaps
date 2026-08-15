using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = Context.User?.FindFirst("fullname")?.Value;
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            var workshopId = Context.User?.FindFirst("WorkshopId")?.Value;

            Console.WriteLine($"✅ SignalR Connected:");
            Console.WriteLine($"   - User ID: {userId}");
            Console.WriteLine($"   - Workshop ID: {workshopId ?? "None"}");

            if (!string.IsNullOrEmpty(workshopId))
            {
                var groupName = $"Workshop_{workshopId}";
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                Console.WriteLine($"   -> Added to Group: {groupName}");
            }

            if (role == "Admin")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            Console.WriteLine($"❌ SignalR Disconnected - User ID: {userId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}