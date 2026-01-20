using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                throw new KeyNotFoundException("Người dùng không tồn tại.");
            }
            user.SetPassword(BCrypt.Net.BCrypt.HashPassword(request.NewPassword));
            return Result.Success();
        }
    }
}
