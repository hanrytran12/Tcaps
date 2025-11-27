using Application.Common;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest
{
    public class UpdateAssignmentTransferRequestCommandHanler : IRequestHandler<UpdateAssignmentTransferRequestCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentTransferRequestRepository _assignmentTransferRequestRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IAppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAssignmentTransferRequestCommandHanler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IBatchRepository batchRepository, IAppDbContext appDbContext,
            IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _batchRepository = batchRepository;
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var transferRequest = await _assignmentTransferRequestRepository.GetByIdAsync(request.TransferRequestId);
            var supplier = await _appDbContext.Users.FindAsync(request.SupplierId);
            if (supplier is null)
            {
                return Result.Failure("Không tìm thấy người vận chuyển.");
            }

            if (transferRequest.ReworkRequestId == null)
            {
                if (transferRequest is null || (transferRequest.Status != "PendingApproval" && transferRequest.Status != "QCTransportReception"))
                {
                    return Result.Failure("Yêu cầu không hợp lệ hoặc đã được duyệt");
                }

                var assigment = await _assignmentRepository.GetByIdAsync(transferRequest.AssignmentId);
                if (assigment is null)
                {
                    return Result.Failure("Không tìm thấy công đoạn");
                }

                if (assigment.Quantity == transferRequest.CompletedQuantity)
                {
                    assigment.UpdateStatus("Completed");
                    assigment.UpdateDateComplete();
                }

                var batch = await _batchRepository.GetByAssignmentIdAsync(assigment.Id);
                if (batch is null)
                {
                    return Result.Failure("Không tìm thấy lô hàng");
                }

                batch.ActiveNextAssignment(assigment.Id, transferRequest.CompletedQuantity);

                transferRequest.MarkAsApproved(request.SupplierId);
            }
            else
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.Id == transferRequest.ReworkRequestId).FirstOrDefaultAsync();
                reworkRequest.Completed();

                var assignment = await _assignmentRepository.GetByIdAsync(reworkRequest.AssignmentId);
                assignment.UpdateStatus("Completed");
                assignment.UpdateDateComplete();

                reworkRequest.AddDomainEvent(new ReworkRequestCompletedEvent(reworkRequest.Id));

                transferRequest.MarkAsApproved(request.SupplierId);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
