using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
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
        private readonly IFinalTransferRequestRepository _finalTransferRequestRepository;
        private readonly IWorkshopRepository _workshopRepository;

        public AddAssignmentTransferRequestCommandHandler(IAssignmentRepository assignmentRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository, IAssignmentCompletionService assignmentCompletionService, IBatchRepository batchRepository,
            IFinalTransferRequestRepository finalTransferRequestRepository, IWorkshopRepository workshopRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _assignmentCompletionService = assignmentCompletionService;
            _batchRepository = batchRepository;
            _finalTransferRequestRepository = finalTransferRequestRepository;
            _workshopRepository = workshopRepository;
        }

        public async Task<Result<Guid>> Handle(AddAssignmentTransferRequestCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (assignment is null)
            {
                throw new NotFoundException("Không tìm thấy công đoạn.");
            }

            //if (assignment.Status != "ReadyForTransfer")
            //{
            //    return Result<Guid>.Failure("Công đoạn này chưa sẵn sàng để chuyển giao.");
            //}

            var batch = await _batchRepository.GetByIdAssignmentWithMaterialUse(request.AssignmentId);

            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng của công đoạn này.");
            }

            var workshop = await _workshopRepository.GetByIdAsync(assignment.WorkshopId);
            if (workshop is null)
            {
                throw new NotFoundException("Không tìm thấy xưởng phụ trách.");
            }

            if (request.ReconciliationMaterials.Count > 0)
            {
                foreach (var item in request.ReconciliationMaterials)
                {
                    batch.UpdateMaterialUsage(request.AssignmentId, item.MaterialId, item.ReconciliationQuantity, request.UserId);
                }
            }

            decimal completedQuantitySend = 0;

            if (assignment.Status == "Reworking")
            {
                var summary = await _assignmentCompletionService.CalculateCompetedQuantityAsync(request.AssignmentId, request.ReworkRequestId);
                completedQuantitySend = summary.TotalCompleted;
            }

            else
            {
                var summary = await _assignmentCompletionService.CalculateCompetedQuantityAsync(request.AssignmentId, null);
                completedQuantitySend = summary.TotalCompleted;
            }
            //var completedQuantity = await _assignmentCompletionService.CalculateCompetedQuantityAsync(request.AssignmentId);

            AssignmentTransferRequest assignmentTransferRequest;
            if (batch.UserId == null && workshop.WorkshopType == Domain.Enums.WorkshopType.Outsource)
            {
                assignmentTransferRequest = AssignmentTransferRequest.Create(
                    assignment.Id,
                    request.UserId,
                    completedQuantitySend,
                    request.Note,
                    null);

                await _assignmentTransferRequestRepository.AddAsync(assignmentTransferRequest);

                assignmentTransferRequest.MarkAsApproved();

                batch.ActiveNextAssignment(assignmentTransferRequest.Id, assignment.Id, completedQuantitySend);
            }
            else
            {
                assignmentTransferRequest = AssignmentTransferRequest.Create(request.AssignmentId, request.UserId, completedQuantitySend, request.Note, (assignment.Status == "Reworking" ? request.ReworkRequestId : null));
                await _assignmentTransferRequestRepository.AddAsync(assignmentTransferRequest);

                assignmentTransferRequest.AddDomainEvent(new TransferRequestAddedEvent(request.UserId, request.AssignmentId));
            }
            

            return Result<Guid>.Success(assignmentTransferRequest.Id);
        }
    }
}
