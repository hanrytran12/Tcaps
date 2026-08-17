using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result<VerifyOtpResponseDTO>>
    {
        private readonly IOtpService _otpService;

        public VerifyOtpCommandHandler(IOtpService otpService)
        {
            _otpService = otpService;
        }

        public async Task<Result<VerifyOtpResponseDTO>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            bool isValid = await _otpService.VerifyOtpAsync(request.Email, request.OtpCode);
            if (!isValid)
            {
                return Result<VerifyOtpResponseDTO>.Failure("Mã OTP không đúng hoặc đã hết hạn.", "invalid_otp");
            }

            string resetToken = await _otpService.CreateResetTokenAsync(request.Email);
            return Result<VerifyOtpResponseDTO>.Success(new VerifyOtpResponseDTO("Xác thực thành công.", resetToken));
        }
    }
}
