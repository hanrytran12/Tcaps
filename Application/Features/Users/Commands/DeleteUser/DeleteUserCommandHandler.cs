using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IAssignmentRepository _assignmentRepository;

        public DeleteUserCommandHandler(IUserRepository repository, IAssignmentRepository assignmentRepository)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                return Result.Failure($"Không tìm thấy User với Id: {request.Id}.");
            }

            var isWorkshopBusy = await _assignmentRepository.HasActiveAssignmentByWorkshopIdAsync(user.WorkshopId);

            if (isWorkshopBusy)
            {
                return Result.Failure("Không thể xóa nhân viên này vì xưởng của họ đang có công đoạn sản xuất.");
            }

            user.MarkAsDeleted();

            return Result.Success();
        }
    }
}
