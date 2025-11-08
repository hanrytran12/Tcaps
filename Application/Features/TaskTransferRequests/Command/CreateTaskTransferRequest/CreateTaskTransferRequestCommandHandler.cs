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

namespace Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest
{
    public class CreateTaskTransferRequestCommandHandler : IRequestHandler<CreateTaskTransferRequestCommand, Result<Guid>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public CreateTaskTransferRequestCommandHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUnitOfWork unitOfWork, 
            IMediator mediator, IUserRepository userRepository)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(CreateTaskTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var taskTranfer = TaskTransferRequest.Create(
                request.BatchId,
                request.WorkshopId,
                request.QcTransportId,
                request.Note);
            await _taskTransferRequestRepository.AddAsync(taskTranfer);
            
            //Bật cờ IsQcTransport = true cho user được phân công
            var qcTransportUser = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransportUser != null)
            {
                qcTransportUser.MarkAsQcTransport();
                _userRepository.Update(qcTransportUser);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CreateTaskTransferRequestEvent(
                taskTranfer.BatchId,
                taskTranfer.WorkshopId,
                taskTranfer.QcTransportId,
                taskTranfer.Note));

            return Result<Guid>.Success(taskTranfer.Id);
        }
    }
}
