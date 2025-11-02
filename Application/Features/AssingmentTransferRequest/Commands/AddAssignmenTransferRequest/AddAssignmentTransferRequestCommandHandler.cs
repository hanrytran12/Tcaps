using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest
{
    public class AddAssignmentTransferRequestCommandHandler : IRequestHandler<AddAssignmentTransferRequestCommand, Result<Guid>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentTransferRequestRepository _assignmentTransferRequestRepository;
        private readonly IAssignmentCompletionService _assignmentCompletionService;

        public AddAssignmentTransferRequestCommandHandler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IAssignmentCompletionService assignmentCompletionService)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _assignmentCompletionService = assignmentCompletionService;
        }

        public async Task<Result<Guid>> Handle(AddAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (assignment is null)
            {
                return Result<Guid>.Failure("Assignment is not exist");
            }

            if (assignment.Status != "ReadyForTransfer")
            {
                return Result<Guid>.Failure("Công đoạn này chưa sẵn sàng để chuyển giao.");
            }

            var completedQuantity = await _assignmentCompletionService.CalculateCompetedQuantityAsync(request.AssignmentId);

            var requestTransfer = AssignmentTransferRequest.Create(request.AssignmentId, request.UserId, completedQuantity, request.Note);
            await _assignmentTransferRequestRepository.AddAsync(requestTransfer);
            return Result<Guid>.Success(requestTransfer.Id);
        }
    }
}
