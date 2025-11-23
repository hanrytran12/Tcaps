using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.QcTransportReception
{
    public class QcTransportReceptionCommandHandler : IRequestHandler<QcTransportReceptionCommand, Result<Guid>>
    {
        private readonly IAssignmentTransferRequestRepository _repository;
        private readonly IMediator _mediator;

        public QcTransportReceptionCommandHandler(IAssignmentTransferRequestRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(QcTransportReceptionCommand request, CancellationToken cancellationToken)
        {
            var assignmentTransferRequest = await _repository.GetByIdAsync(request.AssignmentTransferRequestId);
            if (assignmentTransferRequest == null)
            {
                return Result<Guid>.Failure("Assignment Transfer Request không tìm thấy.");
            }

            if (assignmentTransferRequest.Status == "QCTransportReception")
            {
                return Result<Guid>.Failure("Yêu cầu đã được tiếp nhận");
            }

            assignmentTransferRequest.MarkAsReception();
            _repository.Update(assignmentTransferRequest);

            await _mediator.Publish(new QCTransportReceptionAssignmentTransferEvent(
                request.QCTransportId,
                assignmentTransferRequest.Id, 
                assignmentTransferRequest.AssignmentId), cancellationToken);

            return Result<Guid>.Success(assignmentTransferRequest.Id);
        }
    }
}
