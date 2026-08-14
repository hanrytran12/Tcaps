using Application.Common;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDTO>>
    {
        private const string DefaultRegisteredRole = "Staff";

        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IWorkshopRepository workshopRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var phone = request.Phone.Trim();
            var fullName = request.FullName.Trim();

            if (await _userRepository.DoesEmailExistAsync(email))
            {
                throw new ConflictException("Email này đã được sử dụng.");
            }

            if (await _userRepository.DoesPhoneExistAsync(phone))
            {
                throw new ConflictException("Số điện thoại này đã được sử dụng.");
            }

            if (request.WorkshopId.HasValue && !await _workshopRepository.ExistsAsync(request.WorkshopId))
            {
                throw new BadRequestException("Xưởng không tồn tại");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);
            var user = User.Create(request.WorkshopId, DefaultRegisteredRole, fullName, email, passwordHash, phone);

            await _userRepository.AddAsync(user);

            return Result<AuthResponseDTO>.Success(_jwtTokenGenerator.GenerateToken(user));
        }
    }
}
