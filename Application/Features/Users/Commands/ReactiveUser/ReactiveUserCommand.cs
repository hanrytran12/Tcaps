using Application.Common;
using MediatR;

namespace Application.Features.Users.Commands.ReactiveUser
{
    public class ReactiveUserCommand : IRequest<Result>
    {
        public Guid UserId { get; set; }

        public ReactiveUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
