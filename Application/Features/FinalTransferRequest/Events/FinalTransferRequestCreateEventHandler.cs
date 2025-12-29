using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FinalTransferRequest.Events
{
    public class FinalTransferRequestCreateEventHandler : INotificationHandler<FinalTransferRequestCreatedEvent>
    {
        private readonly IFinalTransferRequestRepository _finalTransferRequestRepository;
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FinalTransferRequestCreateEventHandler(IFinalTransferRequestRepository finalTransferRequestRepository, IMediator mediator, IAppDbContext context, IUnitOfWork unitOfWork)
        {
            _finalTransferRequestRepository = finalTransferRequestRepository;
            _mediator = mediator;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(FinalTransferRequestCreatedEvent notification, CancellationToken cancellationToken)
        {
            var finalTransfer = new Domain.Entities.FinalTransferRequest(
                notification.Id,
                notification.AssignTransferRequestId,
                notification.QuantityFinalSend);

            await _finalTransferRequestRepository.AddAsync(finalTransfer);
            await _unitOfWork.SaveChangesAsync();

            var assignment = await (from at in _context.AssignmentTransferRequests
                                    join a in _context.Assignments on at.AssignmentId equals a.Id
                                    where at.Id == notification.AssignTransferRequestId
                                    select a)
                                    .FirstOrDefaultAsync();

            var workshop = await _context.Workshop.FindAsync(assignment.WorkshopId);
            var batch = await _context.Batches.FindAsync(assignment.BatchId);
            await _mediator.Publish(new FinalTransferRequestForGuardQCEvent(
                notification.QuantityFinalSend,
                batch.Code,
                workshop.Name));
        }
    }
}
