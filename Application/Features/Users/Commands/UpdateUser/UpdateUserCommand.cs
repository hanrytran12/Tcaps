using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? Role { get; set; } = null;
        public string? FullName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Phone { get; set; } = null;
    }
}
