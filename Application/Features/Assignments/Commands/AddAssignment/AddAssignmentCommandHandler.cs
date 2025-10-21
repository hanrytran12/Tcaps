using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.AddAssignmentCommand
{
    public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand, Result<Guid>>
    {
        private readonly IBatchRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public AddAssignmentCommandHandler(IBatchRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = new Assignment(Guid.NewGuid(), request.BatchId, request.WorkshopId, request.Quantity, request.StartDate, request.EndDate);
            var batch = _repository.GetByIdAsync(request.BatchId).Result;
            batch.AddAssignment(assignment);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(assignment.Id);
        }
    }
}
