using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Auth.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public LoginQueryHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailOrPhoneAsync(request.EmailOrPhone);

            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new NotFoundException("Thông tin đăng nhập không chính xác.");
            }

            var tokenResponse = _jwtTokenGenerator.GenerateToken(user);
            return new AuthResponseDTO
            {
                Token = tokenResponse.Token,
                ExpiresAt = tokenResponse.ExpiresAt,
            };
        }
    }
}
