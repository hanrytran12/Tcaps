using Application.DTOs.Response;
using Application.Features.Auth.Queries;
using Application.Interfaces;
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

        public AuthController(IMediator mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<AuthRepsponseDTO> LoginAsync([FromQuery] LoginQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            // 1. Kiểm tra email có tồn tại trong DB không
            // var user = await _userService.FindByEmailAsync(request.Email);
            // if (user == null) return NotFound("Email không tồn tại");

            // 2. Tạo mã OTP (ví dụ 6 số)
            string otpCode = new Random().Next(100000, 999999).ToString();

            // 3. Lưu OTP vào Cache/DB (Code này bạn tự xử lý logic lưu nhé)
            // await _otpService.SaveOtpAsync(request.Email, otpCode);

            // 4. Gửi Email qua Brevo
            // Giả sử tên user là "Nguyen Van A"
            bool isSent = await _emailService.SendOtplEmailAsync(request.Email, "Trần Hoàng Huy", otpCode);

            if (isSent)
            {
                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
            }
            else
            {
                return StatusCode(500, "Có lỗi xảy ra khi gửi email.");
            }
        }
    }
}
