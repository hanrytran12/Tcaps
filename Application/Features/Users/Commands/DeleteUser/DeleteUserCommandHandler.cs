using Application.Common;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppDbContext _context;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository repository, IAppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _context = context;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                return Result.Failure($"Không tìm thấy User với Id: {request.Id}.");
            }

            var query = _context.Users.AsNoTracking().Where(u => u.Id == request.Id && u.Status != "Inactive");
            query = from u in query
                    join w in _context.Workshop on u.WorkshopId equals w.Id
                    select u;

            query = from u in query
                    join a in _context.Assignments on u.WorkshopId equals a.WorkshopId
                    where a.Status == "InProgress"
                    select u;

            if (query.Any())
            {
                return Result.Failure("Cant delete user are doing their work.");
            }

            user.MarkAsDeleted();
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
