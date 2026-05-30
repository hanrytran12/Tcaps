using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;

        public AuthController(IMediator mediator, IEmailService emailService, IUserRepository userRepository, IOtpService otpService)
        {
            _mediator = mediator;
            _emailService = emailService;
            _userRepository = userRepository;
            _otpService = otpService;
        }

        [HttpGet]
        public async Task<AuthRepsponseDTO> LoginAsync([FromQuery] LoginQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthRepsponseDTO>> RegisterAsync([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.Value);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
            {
                return NotFound("Email không tồn tại");
            }

            string otpCode = new Random().Next(100000, 999999).ToString();

            await _otpService.SaveOtpAsync(request.Email, otpCode);

            bool isSent = await _emailService.SendOtplEmailAsync(request.Email, user.FullName, otpCode);

            if (isSent)
            {
                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
            }
            else
            {
                return StatusCode(500, "Có lỗi xảy ra khi gửi email.");
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDTO request)
        {
            bool isValid = await _otpService.VerifyOtpAsync(request.Email, request.OtpCode);

            if (!isValid)
            {
                return BadRequest("Mã OTP không đúng hoặc đã hết hạn.");
            }

            string resetToken = await _otpService.CreateResetTokenAsync(request.Email);

            return Ok(new
            {
                message = "Xác thực thành công.",
                token = resetToken
            });
        }

        [HttpPost("reset-password")]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            string? userEmail = await _otpService.GetEmailByResetTokenAsync(request.Token);

            if (userEmail == null)
            {
                return BadRequest("Phiên đổi mật khẩu đã hết hạn hoặc không hợp lệ. Vui lòng thử lại từ đầu.");
            }

            await _mediator.Send(new Application.Features.Auth.Commands.ResetPassword.ResetPasswordCommand
            {
                Email = userEmail,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            });

            await _otpService.RevokeResetTokenAsync(request.Token);

            return Ok(new { message = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập lại." });
        }
    }
}
