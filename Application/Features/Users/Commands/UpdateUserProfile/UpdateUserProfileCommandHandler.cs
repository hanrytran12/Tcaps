using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProfileCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                return Result.NotFound($"Không tìm thấy User với Id: {request.Id}.");
            }

            if (!string.IsNullOrEmpty(request.Email) && user.Email != request.Email)
            {
                if (await _userRepository.IsEmailTakenByAnotherUserAsync(request.Email, user.Id))
                {
                    return Result.Failure("Email mới đã được sử dụng bởi tài khoản khác");
                }
            }

            if (!string.IsNullOrEmpty(request.Phone) && user.Phone != request.Phone)
            {
                if (await _userRepository.IsPhoneTakenByAnotherUserAsync(request.Phone, user.Id))
                {
                    return Result.Failure("Phone mới đã được sử dụng bởi tài khoản khác");
                }
            }

            user.UpdateProfile(request.FullName, request.Email, request.Phone);
            return Result.Success();
        }
    }
}
