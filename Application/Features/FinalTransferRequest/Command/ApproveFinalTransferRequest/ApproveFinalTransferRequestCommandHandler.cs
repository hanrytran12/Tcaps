using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FinalTransferRequest.Command.ApproveFinalTransferRequest
{
    public class ApproveFinalTransferRequestCommandHandler : IRequestHandler<ApproveFinalTransferRequestCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;
        private readonly IMediator _mediator;
        private readonly IBatchRepository _batchRepository;
        private readonly IAssignmentCompletionService _assignmentCompletionService;

        public ApproveFinalTransferRequestCommandHandler(IAppDbContext context, IMediator mediator, IBatchRepository batchRepository, IAssignmentCompletionService assignmentCompletionService)
        {
            _context = context;
            _mediator = mediator;
            _batchRepository = batchRepository;
            _assignmentCompletionService = assignmentCompletionService;
        }
        public async Task<Result<Guid>> Handle(ApproveFinalTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var finalRequest = await _context.FinalTransferRequests.FindAsync(request.Id);
            if (finalRequest is null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu chuyển giao cuối cùng");
            }

            if (request.QuantityFinalReceive < 0 || request.QuantityFinalReceive > finalRequest.QuantityFinalSend)
            {
                throw new BadRequestException("Số lượng nhận không hợp lệ");
            }

            var assignTransfer = await _context.AssignmentTransferRequests.FindAsync(finalRequest.AssignTransferRequestId);
            if (assignTransfer is null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu chuyển giao");
            }

            var batch = await _batchRepository.GetByAssignmentIdAsync(assignTransfer.AssignmentId);
            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng");
            }

            var summary = await _assignmentCompletionService.CalculateCompetedQuantityAsync(assignTransfer.AssignmentId, null);

            var quantityError = summary.TotalRejected + (finalRequest.QuantityFinalSend - request.QuantityFinalReceive);
            
            finalRequest.Approve(request.QuantityFinalReceive, request.Note);

            batch.CompleteBatch(assignTransfer.CompletedQuantityReceive, quantityError);

            await _mediator.Publish(new ApproveFinalTransferRequestEvent(
                batch.Id,
                finalRequest.QuantityFinalSend,
                quantityError));

            return Result<Guid>.Success(finalRequest.Id);
        }
    }
}
