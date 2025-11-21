using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
