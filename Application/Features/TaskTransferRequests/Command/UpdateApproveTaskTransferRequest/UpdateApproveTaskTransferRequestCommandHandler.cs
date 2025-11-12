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
        private readonly IUserRepository _userRepository;

        public UpdateApproveTaskTransferRequestCommandHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUnitOfWork unitOfWork, IMediator mediator, IUserRepository userRepository)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateApproveTaskTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var taskTransfer = await _taskTransferRequestRepository.GetByIdAsync(request.TaskTransferId);

            taskTransfer.UpdateApproveStatus();
            _taskTransferRequestRepository.Update(taskTransfer);

            //Bật cờ IsQcTransport = true cho user được phân công
            var qcTransportUser = await _userRepository.GetByIdAsync(taskTransfer.QcTransportId);
            if (qcTransportUser != null)
            {
                qcTransportUser.MarkAsQcTransport();
                _userRepository.Update(qcTransportUser);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new ApproveTaskTransferRequestEvent(
                taskTransfer.Id,
                taskTransfer.QcTransportId));

            return Result<Guid>.Success(taskTransfer.Id);
        }
    }
}
