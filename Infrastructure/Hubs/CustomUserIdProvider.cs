using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            // Lấy User ID từ 'sub' claim hoặc ClaimTypes.NameIdentifier
            var userId = connection.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            Console.WriteLine($"🔍 SignalR GetUserId: {userId}");
            return userId;
        }
    }
}
