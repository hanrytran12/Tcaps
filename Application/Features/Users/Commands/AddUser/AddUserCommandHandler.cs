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
            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(request.WorkshopId, request.Role, request.FullName, request.Email,
                passwordHash, request.Phone);
            await _repository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(user.Id);
        }
    }
}
