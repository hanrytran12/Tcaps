using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                      ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }
    }
}
