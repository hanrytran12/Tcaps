using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class TransferRequestInProgressEventHandler : INotificationHandler<TransferRequestInProgressEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;
        private readonly IMediator _mediator;

        public TransferRequestInProgressEventHandler(IAppDbContext appDbContext, IMaterialWorkshopRepository materialWorkshopRepository, IMediator mediator)
        {
            _appDbContext = appDbContext;
            _materialWorkshopRepository = materialWorkshopRepository;
            _mediator = mediator;
        }

        public async Task Handle(TransferRequestInProgressEvent notification, CancellationToken cancellationToken)
        {
            var asignmentTransferRequest = await _appDbContext.AssignmentTransferRequests
                .Where(atr => atr.AssignmentId == notification.AssignmentId).FirstOrDefaultAsync();

            if (asignmentTransferRequest is null)
            {
                throw new NotFoundException("Không tìm thấy đơn chuyển giao.");
            }
            var assignment = await _appDbContext.Assignments
                .Where(a => a.Id == notification.AssignmentId)
                .FirstOrDefaultAsync();

            if (assignment is null)
            {
                throw new NotFoundException("Không tìm thấy công đoạn.");
            }

            var batch = await _appDbContext.Batches.FindAsync(assignment.BatchId);

            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng.");
            }

            var reworkRequest = await _appDbContext.ReworkRequests
                .FirstOrDefaultAsync(rr => rr.AssignmentId == notification.AssignmentId, cancellationToken);
            if (reworkRequest is null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu làm lại");
            }

            reworkRequest.UpdateDefectiveQuantity(
                notification.QuantityReject,
                asignmentTransferRequest.UserId,
                batch.Code);

            notification.QuantitySend = reworkRequest.DefectiveQuantity;

            var currentStepOrder = assignment.StepOrder;
            var nextAssigment = await _appDbContext.Assignments.Where(a => a.StepOrder > currentStepOrder && a.BatchId == assignment.BatchId).OrderBy(a => a.StepOrder).FirstOrDefaultAsync();

            if (nextAssigment is not null)
            {
                var materialWorkshop = MaterialWorkshop.Create(nextAssigment.WorkshopId, notification.AssignmentId, notification.AssignmentTransferRequestId, notification.SupplierId, (int)notification.QuantitySend);
                await _materialWorkshopRepository.AddAsync(materialWorkshop);
            }
        }
    }
}
