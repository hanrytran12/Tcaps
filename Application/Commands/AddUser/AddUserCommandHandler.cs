using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUserRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

            var user = new User(Guid.NewGuid(), null, request.Role, request.FullName, request.Email, (request.Password == request.PasswordConfirmed) ? request.Password : "", request.Phone);
            await _repository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(user.Id);
        }
    }
}
