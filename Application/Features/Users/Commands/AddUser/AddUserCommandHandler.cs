using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWorkshopRepository _workshopRepository;

        public AddUserCommandHandler(IUserRepository repository,
            IPasswordHasher passwordHasher, IWorkshopRepository workshopRepository)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _workshopRepository = workshopRepository;
        }

        public async Task<Result<Guid>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.DoesEmailExistAsync(request.Email))
            {
                return Result<Guid>.Failure("Email này đã được sử dụng.");
            }

            if (await _repository.DoesPhoneExistAsync(request.Phone))
            {
                return Result<Guid>.Failure("Số điện thoại này đã được sử dụng.");
            }

            if (!await _workshopRepository.ExistsAsync(request.WorkshopId))
            {
                return Result<Guid>.Failure("Xưởng không tồn tại");
            }

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = User.Create(request.WorkshopId, request.Role, request.FullName, request.Email,
                passwordHash, request.Phone);

            await _repository.AddAsync(user);
            return Result<Guid>.Success(user.Id);
        }
    }
}
