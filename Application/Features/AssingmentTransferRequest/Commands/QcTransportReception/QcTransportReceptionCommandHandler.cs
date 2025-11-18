using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.QcTransportReception
{
    public class QcTransportReceptionCommandHandler : IRequestHandler<QcTransportReceptionCommand, Result<Guid>>
    {
        private readonly IAssignmentTransferRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public QcTransportReceptionCommandHandler(IAssignmentTransferRequestRepository repository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(QcTransportReceptionCommand request, CancellationToken cancellationToken)
        {
            var assignmentTransferRequest = await _repository.GetByIdAsync(request.AssignmentTransferRequestId);
            if (assignmentTransferRequest == null)
            {
                return Result<Guid>.Failure("Assignment Transfer Request not found.");
            }

            assignmentTransferRequest.MarkAsApproved();
            _repository.Update(assignmentTransferRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new QCTransportReceptionAssignmentTransferEvent(
                request.QCTransportId,
                assignmentTransferRequest.Id, 
                assignmentTransferRequest.AssignmentId), cancellationToken);

            return Result<Guid>.Success(assignmentTransferRequest.Id);
        }
    }
}
