using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Common;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries;
using Application.Interfaces;
using API.Contracts;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Cryptography;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public AuthController(
            ISender mediator,
            IEmailService emailService,
            IUserRepository userRepository,
            IOtpService otpService) : base(mediator)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _otpService = otpService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDTO>> LoginAsync([FromBody] LoginQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommand command)
        {
            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return HandleResult(Result.NotFound("Email không tồn tại"));
            }

            string otpCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            await _otpService.SaveOtpAsync(request.Email, otpCode);

            bool isSent = await _emailService.SendOtpEmailAsync(request.Email, user.FullName, otpCode);

            if (isSent)
            {
                return SuccessMessage("Mã OTP đã được gửi đến email của bạn.");
            }

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiErrorResponse(
                    "email_delivery_failed",
                    "Có lỗi xảy ra khi gửi email.",
                    StatusCodes.Status500InternalServerError,
                    HttpContext.TraceIdentifier));
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDTO request)
        {
            bool isValid = await _otpService.VerifyOtpAsync(request.Email, request.OtpCode);

            if (!isValid)
            {
                return HandleResult(Result.Failure("Mã OTP không đúng hoặc đã hết hạn.", "invalid_otp"));
            }

            string resetToken = await _otpService.CreateResetTokenAsync(request.Email);

            return Ok(new ApiTokenResponse("Xác thực thành công.", resetToken));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            string? userEmail = await _otpService.GetEmailByResetTokenAsync(request.Token);

            if (userEmail == null)
            {
                return HandleResult(Result.Failure(
                    "Phiên đổi mật khẩu đã hết hạn hoặc không hợp lệ. Vui lòng thử lại từ đầu.",
                    "invalid_reset_token"));
            }

            var result = await Mediator.Send(new Application.Features.Auth.Commands.ResetPassword.ResetPasswordCommand
            {
                Email = userEmail,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            });

            if (!result.IsSuccess)
            {
                return HandleResult(result);
            }

            await _otpService.RevokeResetTokenAsync(request.Token);

            return SuccessMessage("Đặt lại mật khẩu thành công. Vui lòng đăng nhập lại.");
        }
    }
}
