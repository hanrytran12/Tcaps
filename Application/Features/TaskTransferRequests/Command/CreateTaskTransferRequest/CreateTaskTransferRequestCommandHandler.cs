using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest
{
    public class CreateTaskTransferRequestCommandHandler : IRequestHandler<CreateTaskTransferRequestCommand, Result<Guid>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public CreateTaskTransferRequestCommandHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }
        public async Task<Result<Guid>> Handle(CreateTaskTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var taskTranfer = TaskTransferRequest.Create(
                request.BatchId,
                request.WorkshopId,
                request.QcTransportId,
                request.MaterialRequestId,
                request.AssignmentTransferId,
                request.Note,
                request.DateToGo);
            await _taskTransferRequestRepository.AddAsync(taskTranfer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _notificationService.NotifyAdminDashboardRefreshAsync(taskTranfer.Id);

            return Result<Guid>.Success(taskTranfer.Id);
        }
    }
}
