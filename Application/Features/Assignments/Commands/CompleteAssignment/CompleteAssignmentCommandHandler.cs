using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.CompleteAssignment
{
    public class CompleteAssignmentCommandHandler : IRequestHandler<CompleteAssignmentCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CompleteAssignmentCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork)
        {
            _batchRepository = batchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CompleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetAggregateRootByAssignmentIdAsync(request.AssignmentId);
            if (batch is null)
            {
                return Result.Failure("Không tìm thấy lô hàng.");
            }
            batch.UpdateAssignmentsStatus(request.AssignmentId, "Completed");
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
