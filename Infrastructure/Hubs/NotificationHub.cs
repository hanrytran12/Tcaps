using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"🔔 SignalR Connected: UserId = {userId}");
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"✅ User {userId} added to SignalR group");
            }
            else
            {
                Console.WriteLine("❌ No userId found in JWT token!");
            }
            await base.OnConnectedAsync();
        }
    }
}
