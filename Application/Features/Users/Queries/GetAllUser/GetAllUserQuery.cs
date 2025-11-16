using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUser
{
    public class GetAllUserQuery : IRequest<List<UsersDTO>> { }
}
