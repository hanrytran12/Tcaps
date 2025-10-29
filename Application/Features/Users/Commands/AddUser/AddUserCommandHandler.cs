using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWorkshopRepository _workshopRepository;

        public AddUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IWorkshopRepository workshopRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _workshopRepository = workshopRepository;
        }

        public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var isEmailUnique = await _userRepository.DoesEmailExistAsync(request.Email);

            if (isEmailUnique)
            {
                return Result<Guid>.Failure("Email is used by another user.");
            }

            var isPhoneUnique = await _userRepository.DoesPhoneExistAsync(request.Phone);

            if (isPhoneUnique)
            {
                return Result<Guid>.Failure("Phone number is used by another user.");
            }

            var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);

            if (workshop is null)
            {
                return Result<Guid>.Failure("Workshop is not exist.");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(request.WorkshopId, request.Role, request.FullName, request.Email,
                passwordHash, request.Phone);
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(user.Id);
        }
    }
}
