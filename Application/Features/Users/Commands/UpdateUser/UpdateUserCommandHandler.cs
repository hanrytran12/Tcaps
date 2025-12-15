using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new NotFoundException($"Không tìm thấy User với Id: {request.Id}.");
            }

            if (!string.IsNullOrEmpty(request.Email) && user.Email != request.Email)
            {
                if (await _userRepository.IsEmailTakenByAnotherUserAsync(request.Email, user.Id))
                {
                    throw new ConflictException("Email mới đã được sử dụng bởi tài khoản khác");
                }
            }

            if (!string.IsNullOrEmpty(request.Phone) && user.Phone != request.Phone)
            {
                if (await _userRepository.IsPhoneTakenByAnotherUserAsync(request.Phone, user.Id))
                {
                    throw new ConflictException("Phone mới đã được sử dụng bởi tài khoản khác");
                }
            }

            user.UpdateDetails(request.Role, request.FullName, request.Email, request.Phone);
            return Result.Success();
        }
    }
}
