using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponseDTO GenerateToken(User user)
        {
            var issuer = GetRequiredSetting("ISSUER", "JwtSettings:Issuer");
            var audience = GetRequiredSetting("AUDIENCE", "JwtSettings:Audience");
            var secretKeyValue = GetRequiredSetting("JWT_KEY", "JwtSettings:SecretKey");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("fullname", user.FullName),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
                new Claim(JwtRegisteredClaimNames.Aud, audience)
            };

            if (user.WorkshopId.HasValue)
            {
                claims.Add(new Claim("WorkshopId", user.WorkshopId.Value.ToString()));
            }

            if (user.IsQcTransport)
            {
                claims.Add(new Claim("isQcTransport", "true"));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(securityToken);

            return new AuthResponseDTO
            {
                Token = tokenString,
                ExpiresAt = tokenDescriptor.Expires.Value,
            };
        }

        private string GetRequiredSetting(string environmentName, string configurationPath)
        {
            var value = Environment.GetEnvironmentVariable(environmentName)
                ?? _configuration[configurationPath];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Missing JWT configuration. Set {environmentName}.");
            }

            return value;
        }
    }
}
