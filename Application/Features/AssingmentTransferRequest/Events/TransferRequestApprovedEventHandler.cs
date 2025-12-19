using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Events
{
    public class TransferRequestApprovedEventHandler : INotificationHandler<TransferRequestApprovedEvent>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;
        private readonly IMediator _mediator;

        public TransferRequestApprovedEventHandler(IAppDbContext appDbContext, IMaterialWorkshopRepository materialWorkshopRepository, IMediator mediator)
        {
            _appDbContext = appDbContext;
            _materialWorkshopRepository = materialWorkshopRepository;
            _mediator = mediator;
        }

        public async Task Handle(TransferRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            var asignmentTransferRequest = await _appDbContext.AssignmentTransferRequests.Where(atr => atr.AssignmentId == notification.AssignmentId).FirstOrDefaultAsync();
            var assignment = await _appDbContext.Assignments.Where(a => a.Id == notification.AssignmentId).FirstOrDefaultAsync();
            var batch = await _appDbContext.Batches.FindAsync(assignment.BatchId);
            if (asignmentTransferRequest.ReworkRequestId is not null)
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(rr => rr.AssignmentId == notification.AssignmentId).FirstOrDefaultAsync();
                reworkRequest.UpdateDefectiveQuantity(
                    notification.QuantityReject, 
                    asignmentTransferRequest.UserId,
                    batch.Code);

                notification.QuantitySend = reworkRequest.DefectiveQuantity;
            }
            else
            {
                var reworkRequest = await _appDbContext.ReworkRequests.FindAsync(assignment.Id);
                reworkRequest.UpdateDefectiveQuantity(
                    notification.QuantityReject,
                    asignmentTransferRequest.UserId,
                    batch.Code);
            }

            var currentStepOrder = assignment.StepOrder;
            var nextAssigment = await _appDbContext.Assignments.Where(a => a.StepOrder > currentStepOrder && a.BatchId == assignment.BatchId).OrderBy(a => a.StepOrder).FirstOrDefaultAsync();

            if (nextAssigment is not null)
            {
                var materialWorkshop = MaterialWorkshop.Create(nextAssigment.WorkshopId, notification.AssignmentId, notification.SupplierId, (int)notification.QuantitySend);
                await _materialWorkshopRepository.AddAsync(materialWorkshop);
            }
        }
    }
}
