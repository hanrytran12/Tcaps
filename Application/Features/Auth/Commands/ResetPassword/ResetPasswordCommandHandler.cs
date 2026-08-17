using Application.Common;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IOtpService otpService,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            string? userEmail = await _otpService.GetEmailByResetTokenAsync(request.Token);
            if (userEmail == null)
            {
                return Result.Failure("Phiên đổi mật khẩu đã hết hạn hoặc không hợp lệ. Vui lòng thử lại từ đầu.", "invalid_reset_token");
            }

            var user = await _userRepository.GetByEmailAsync(userEmail);
            if (user is null)
            {
                return Result.NotFound("Người dùng không tồn tại.", "user_not_found");
            }

            var newHash = _passwordHasher.Hash(request.NewPassword);
            user.SetPassword(newHash);

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _otpService.RevokeResetTokenAsync(request.Token);

            return Result.Success();
        }
    }
}
