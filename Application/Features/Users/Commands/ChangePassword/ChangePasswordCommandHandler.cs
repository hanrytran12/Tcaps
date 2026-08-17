using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.NotFound("User not found", "user_not_found");
            }

            if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return Result.Failure("Current password is incorrect", "invalid_current_password");
            }

            var newHash = _passwordHasher.Hash(request.NewPassword);
            user.ChangePassword(request.CurrentPassword, newHash, _passwordHasher);

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
