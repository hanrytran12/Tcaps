using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Features.Auth.Queries;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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

            return Ok("Xác thực thành công.");
        }
    }
}
