using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Auth.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthRepsponseDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthRepsponseDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailOrPhoneAsync(request.EmailOrPhone);

            if (user is null)
            {
                return Result<AuthRepsponseDTO>.Failure("Thông tin đăng nhập không chính xác.");
            }

            bool isPasswordVaid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordVaid)
            {
                return Result<AuthRepsponseDTO>.Failure("Wrong password.");
            }

            var tokenResponse = _jwtTokenGenerator.GenerateToken(user);
            return Result<AuthRepsponseDTO>.Success(tokenResponse);
        }
    }
}
