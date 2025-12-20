using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IProductionRepository _productionRepository;

        public DeleteUserCommandHandler(IUserRepository repository, IAssignmentRepository assignmentRepository, IProductionRepository productionRepository)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
            _productionRepository = productionRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            if (user is null)
            {
                throw new NotFoundException($"Không tìm thấy User với Id: {request.Id}.");
            }

            var productions = await _productionRepository.GetByUserAsync(user.Id);
            if (productions.Any())
            {
                user.MarkAsDeleted();
            }
            else
            {
                _repository.Delete(user);
            }

            return Result.Success();
        }
    }
}
