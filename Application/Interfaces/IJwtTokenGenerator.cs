using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        AuthResponseDTO GenerateToken(User user);
    }
}
