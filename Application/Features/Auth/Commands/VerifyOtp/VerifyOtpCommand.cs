using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Auth.Commands.VerifyOtp
{
    public record VerifyOtpCommand(string Email, string OtpCode) : IRequest<Result<VerifyOtpResponseDTO>>;
}
