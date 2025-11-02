using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest
{
    public class UpdateAssignmentTransferRequestCommandHanler : IRequestHandler<UpdateAssignmentTransferRequestCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentTransferRequestRepository _assignmentTransferRequestRepository;
        private readonly IBatchRepository _batchRepository;

        public UpdateAssignmentTransferRequestCommandHanler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IBatchRepository batchRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(UpdateAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var transferRequest = await _assignmentTransferRequestRepository.GetByIdAsync(request.TransferRequestId);
            if (transferRequest is null || transferRequest.Status != "PendingApproval")
            {
                return Result.Failure("Yêu cầu không hợp lệ hoặc đã được duyệt");
            }

            var assigment = await _assignmentRepository.GetByIdAsync(transferRequest.AssignmentId);
            if (assigment is null)
            {
                return Result.Failure("Không tìm thấy công đoạn");
            }

            var batch = await _batchRepository.GetByAssignmentIdAsync(assigment.Id);
            if (batch is null)
            {
                return Result.Failure("Không tìm thấy lô hàng");
            }

            batch.CompleteAndActiveNextAssignment(assigment.Id);
            transferRequest.MarkAsApproved();

            return Result.Success();
        }
    }
}
