using Domain.Entities;
using MediatR;

namespace Application.Queries.GetAllUser
{
    public class GetAllUserQuery : IRequest<List<User>> { }
}
