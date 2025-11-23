using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.CompleteAssignment
{
    public class CompleteAssignmentCommandHandler : IRequestHandler<CompleteAssignmentCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        public CompleteAssignmentCommandHandler(IBatchRepository batchRepository, IUnitOfWork unitOfWork, IAssignmentRepository assignmentRepository, IMediator mediator)
        {
            _batchRepository = batchRepository;
            _unitOfWork = unitOfWork;
            _assignmentRepository = assignmentRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(CompleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetByIdAsync(request.AssignmentId);

            if (assignments is null)
            {
                return Result.Failure("Assignment is not exist.");
            }

            assignments.UpdateStatus("Completed");


            var isBatchCompleted = await _batchRepository.AreAllAssignmentsCompletedAsync(assignments.BatchId);
            if (isBatchCompleted)
            {
                var batch = await _batchRepository.GetByIdAsync(assignments.BatchId);
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
