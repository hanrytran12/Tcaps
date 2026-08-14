using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Auth.Queries
{
    public class LoginQuery : IRequest<AuthResponseDTO>
    {
        public string EmailOrPhone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
