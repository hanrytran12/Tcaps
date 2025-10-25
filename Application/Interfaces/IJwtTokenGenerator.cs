using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        AuthRepsponseDTO GenerateToken(User user);
    }
}
