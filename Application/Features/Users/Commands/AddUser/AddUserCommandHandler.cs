using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public AddUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork, 
            IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.DoesEmailExistAsync(request.Email))
            {
                return Result<Guid>.Failure("Email này đã được đăng ký.");
            }

            if (await _repository.DoesPhoneExistAsync(request.Phone))
            {
                return Result<Guid>.Failure("Phone này đã được đăng ký.");
            }

            if (request.Password != request.PasswordConfirmed)
            {
                return Result<Guid>.Failure("Mật khẩu không khớp.");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User(Guid.NewGuid(), request.WorkshopId, request.Role, request.FullName, request.Email, passwordHash, request.Phone);
            await _repository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(user.Id);
        }
    }
}
