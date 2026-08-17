using Application.Common;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;

namespace Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository,
            IOtpService otpService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.NotFound("Email không tồn tại", "email_not_found");
            }

            string otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            await _otpService.SaveOtpAsync(request.Email, otpCode);

            bool isSent = await _emailService.SendOtpEmailAsync(request.Email, user.FullName, otpCode);
            if (!isSent)
            {
                return Result.Internal("Có lỗi xảy ra khi gửi email.", "email_delivery_failed");
            }

            return Result.Success();
        }
    }
}
