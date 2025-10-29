using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                return Result.Failure($"Không tìm thấy User với Id: {request.Id}.");
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

            user.UpdateDetails(request.Role, request.FullName, request.Email, request.Phone);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
