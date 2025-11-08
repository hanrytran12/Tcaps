using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest
{
    public class UpdateApproveTaskTransferRequestCommandHandler : IRequestHandler<UpdateApproveTaskTransferRequestCommand, Result<Guid>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateApproveTaskTransferRequestCommandHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(UpdateApproveTaskTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var taskTransfer = await _taskTransferRequestRepository.GetByIdAsync(request.TaskTransferId);

            taskTransfer.UpdateApproveStatus();
            _taskTransferRequestRepository.Update(taskTransfer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ApproveTaskTransferRequestEvent(
                taskTransfer.Id,
                taskTransfer.QcTransportId));

            return Result<Guid>.Success(taskTransfer.Id);
        }
    }
}
