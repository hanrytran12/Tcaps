using Application.Common;
using MediatR;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommand : IRequest<Result<Guid>>
    {
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmed { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid WorkshopId { get; set; }
    }
}
