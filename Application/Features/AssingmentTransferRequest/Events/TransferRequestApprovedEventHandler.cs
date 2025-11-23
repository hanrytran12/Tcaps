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

        public TransferRequestApprovedEventHandler(IAppDbContext appDbContext, IMaterialWorkshopRepository materialWorkshopRepository)
        {
            _appDbContext = appDbContext;
            _materialWorkshopRepository = materialWorkshopRepository;
        }

        public async Task Handle(TransferRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            var assignment = await _appDbContext.Assignments.Where(a => a.Id == notification.AssignmentId).FirstOrDefaultAsync(cancellationToken);
            if (assignment.Status == "Reworking")
            {
                var reworkRequest = await _appDbContext.ReworkRequests.Where(rr => rr.AssignmentId == notification.AssignmentId).FirstOrDefaultAsync();
                notification.QuantitySend = reworkRequest.DefectiveQuantity;
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
