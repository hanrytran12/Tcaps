using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.ReactiveUser
{
    public class ReactiveUserCommandHandler : IRequestHandler<ReactiveUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public ReactiveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ReactiveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result.Failure("User not found.");
            }

            user.Reactive();
            return Result.Success();
        }
    }
}
