using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUser
{
    public class GetAllUserQuery : IRequest<List<User>> { }
}
