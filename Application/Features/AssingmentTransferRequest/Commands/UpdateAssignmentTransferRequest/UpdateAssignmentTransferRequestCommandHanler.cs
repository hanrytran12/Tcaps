using Application.Common;
using Application.Common.Exceptions;
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
        private readonly IAssignmentCompletionService _assignmentCompletionService;

        public UpdateAssignmentTransferRequestCommandHanler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IBatchRepository batchRepository, IAppDbContext appDbContext,
            IUnitOfWork unitOfWork, IAssignmentCompletionService assignmentCompletionService)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _batchRepository = batchRepository;
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
            _assignmentCompletionService = assignmentCompletionService;
        }

        public async Task<Result> Handle(UpdateAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var transferRequest = await _assignmentTransferRequestRepository.GetByIdAsync(request.TransferRequestId);
            var supplier = await _appDbContext.Users.FindAsync(request.SupplierId);
            if (supplier is null)
            {
                throw new NotFoundException("Không tìm thấy người vận chuyển.");
            }

            if (transferRequest.ReworkRequestId == null)
            {
                if (transferRequest is null || (transferRequest.Status != "PendingApproval" && transferRequest.Status != "QCTransportReception"))
                {
                    throw new BadRequestException("Yêu cầu không hợp lệ hoặc đã được duyệt");
                }

                var assigment = await _assignmentRepository.GetByIdAsync(transferRequest.AssignmentId);
                if (assigment is null)
                {
                    throw new NotFoundException("Không tìm thấy công đoạn");
                }

                if (assigment.Quantity <= transferRequest.CompletedQuantitySend)
                {
                    assigment.UpdateStatus("Completed");
                    assigment.UpdateDateComplete();
                }

                var batch = await _batchRepository.GetByAssignmentIdAsync(assigment.Id);
                if (batch is null)
                {
                    throw new NotFoundException("Không tìm thấy lô hàng");
                }

                var summary = await _assignmentCompletionService.CalculateCompetedQuantityAsync(assigment.Id, null);
                bool check = batch.ActiveNextAssignment(transferRequest.Id, assigment.Id, transferRequest.CompletedQuantitySend);

                if (check)
                {
                    transferRequest.MarkAsInProgress(request.SupplierId, transferRequest.Id, request.CompleteQuantityReceive, request.NoteLead);
                }
                else
                {
                    transferRequest.MarkAsApprovedFinal(request.CompleteQuantityReceive, request.NoteLead);
                }
            }
            else
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.Id == transferRequest.ReworkRequestId).FirstOrDefaultAsync();
                reworkRequest.Completed();

                var assignment = await _assignmentRepository.GetByIdAsync(reworkRequest.AssignmentId);
                assignment.UpdateStatus("Completed");
                assignment.UpdateDateComplete();

                reworkRequest.AddDomainEvent(new ReworkRequestCompletedEvent(reworkRequest.Id));

                transferRequest.MarkAsInProgress(request.SupplierId, transferRequest.Id, request.CompleteQuantityReceive, request.NoteLead);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
