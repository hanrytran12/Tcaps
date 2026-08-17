using Application.Common;
using MediatR;

namespace Application.Features.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Token, string NewPassword, string ConfirmPassword) : IRequest<Result>;
}
