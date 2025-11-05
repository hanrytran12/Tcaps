using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest
{
    public class AddAssignmentTransferRequestCommandHandler : IRequestHandler<AddAssignmentTransferRequestCommand, Result<Guid>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentTransferRequestRepository _assignmentTransferRequestRepository;
        private readonly IAssignmentCompletionService _assignmentCompletionService;
        private readonly IBatchRepository _batchRepository;

        public AddAssignmentTransferRequestCommandHandler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IAssignmentCompletionService assignmentCompletionService, IBatchRepository batchRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _assignmentCompletionService = assignmentCompletionService;
            _batchRepository = batchRepository;
        }

        public async Task<Result<Guid>> Handle(AddAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (assignment is null)
            {
                return Result<Guid>.Failure("Assignment is not exist");
            }

            if (assignment.Status != "ReadyForTransfer")
            {
                return Result<Guid>.Failure("Công đoạn này chưa sẵn sàng để chuyển giao.");
            }

            var batch = await _batchRepository.GetByIdAssignmentWithMaterialUse(request.AssignmentId);

            if (batch is null)
            {
                return Result<Guid>.Failure("Không tìm thấy lô hàng của công đoạn này.");
            }

            if (request.ReconciliationMaterials.Count > 0)
            {
                foreach (var item in request.ReconciliationMaterials)
                {
                    batch.UpdateMaterialUsage(request.AssignmentId, item.MaterialId, item.ReconciliationQuantity, request.UserId);
                }
            }

            var completedQuantity = await _assignmentCompletionService.CalculateCompetedQuantityAsync(request.AssignmentId);

            var requestTransfer = AssignmentTransferRequest.Create(request.AssignmentId, request.UserId, completedQuantity, request.Note);
            await _assignmentTransferRequestRepository.AddAsync(requestTransfer);
            return Result<Guid>.Success(requestTransfer.Id);
        }
    }
}
