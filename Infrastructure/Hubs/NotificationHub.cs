using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Lấy User ID từ claim "sub"
            var userId = Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userName = Context.User?.FindFirst("fullname")?.Value;
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"✅ SignalR Connected:");
            Console.WriteLine($"   - User ID: {userId}");
            Console.WriteLine($"   - Name: {userName}");
            Console.WriteLine($"   - Role: {role}");
            Console.WriteLine($"   - Connection ID: {Context.ConnectionId}");
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
