using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
