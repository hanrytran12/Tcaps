using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Result<AuthRepsponseDTO>>
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmed { get; set; } = string.Empty;
        public Guid? WorkshopId { get; set; }
    }
}
