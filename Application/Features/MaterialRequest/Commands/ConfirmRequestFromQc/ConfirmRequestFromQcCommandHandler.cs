using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc
{
    public class ConfirmRequestFromQcCommandHandler : IRequestHandler<ConfirmRequestFromQcCommand, Result>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IBatchRepository _batchRepository;

        public ConfirmRequestFromQcCommandHandler(IMaterialRequestRepository materialRequestRepository, IBatchRepository batchRepository, IAppDbContext appDbContext)
        {
            _materialRequestRepository = materialRequestRepository;
            _batchRepository = batchRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Result> Handle(ConfirmRequestFromQcCommand request, CancellationToken cancellationToken)
        {            
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.Id);

            if (materialRequest is null)
            {
                throw new NotFoundException("Material request not found.");
            }

            var taskTransferRequest = await _appDbContext.TaskTransferRequests
                .FindAsync(materialRequest.Id);

            var qcTransport = taskTransferRequest != null
                ? await _appDbContext.Users.FindAsync(taskTransferRequest.QcTransportId)
                : null;

            if (taskTransferRequest is not null && materialRequest.Status != "QCTransportReception")
            {
                throw new BadRequestException("QC xưởng chưa thể duyệt vì QC vận chuyển chưa tiếp nhận");
            }

            if (materialRequest.QuantityRequest - request.ActualReceivedQuantity == 0)
            {
                materialRequest.MarkAsConfirmed(request.ActualReceivedQuantity, request.NoteFromQC);
            }
            else
            {
                materialRequest.MarkAsConfirmedWithDiscrepancy(request.ActualReceivedQuantity, request.NoteFromQC);
            }

            var assignment = await _appDbContext.Assignments.Where(a => a.Id == materialRequest.AssignId).SingleOrDefaultAsync();
            if (assignment.Status == "Reworking")
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.AssignmentId == assignment.Id).SingleOrDefaultAsync();
                reworkRequest.InProgressRequest();
            }
            else
            {
                var batch = await _batchRepository.GetByIdWithAssignmentsAsync(materialRequest.BatchId);
                batch.ConfirmMaterialReceiptForAssignment(materialRequest.AssignId);
            }

            if (qcTransport != null)
            {
                qcTransport.MarkAsQcTransport();
                _appDbContext.Users.Update(qcTransport);
            }

            return Result.Success();
        }
    }
}
