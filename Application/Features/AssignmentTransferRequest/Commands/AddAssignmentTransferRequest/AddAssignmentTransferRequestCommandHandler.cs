using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssignmentTransferRequest.Commands.AddAssignmentTransferRequest
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

            bool isOutsource = workshop.WorkshopType == WorkshopType.Outsource;

            if (isOutsource)
            {
                if (!request.QuantityCompletedSend.HasValue)
                {
                    return Result<Guid>.Failure(
                        "Xưởng khoán bắt buộc phải nhập số lượng hoàn thành."
                    );
                }

                if (request.QuantityCompletedSend.Value <= 0)
                {
                    return Result<Guid>.Failure(
                        "Số lượng hoàn thành phải lớn hơn 0."
                    );
                }
            }

            if (request.ReconciliationMaterials.Count > 0)
            {
                // Validate toàn bộ trước khi update, tránh partial update khi có lỗi
                foreach (var item in request.ReconciliationMaterials)
                {
                    batch.UpdateMaterialUsage(request.AssignmentId, item.MaterialId, item.ReconciliationQuantity, request.UserId);
                }
            }

            decimal quantityToSend = 0;
            if (isOutsource)
            {
                quantityToSend = request.QuantityCompletedSend!.Value;
            }
            else
            {
                var summary = await _assignmentCompletionService.CalculateCompletedQuantityAsync(
                    request.AssignmentId, 
                    assignment.Status == "Reworking" ? request.ReworkRequestId : null);
                
                quantityToSend = summary.TotalCompleted;
            }

            var reworkRequestId = !isOutsource && assignment.Status == "Reworking" ? request.ReworkRequestId : null;
            var assignmentTransferRequest = Domain.Entities.AssignmentTransferRequest.Create(
                assignment.Id,
                request.UserId,
                quantityToSend,
                request.Note,
                reworkRequestId);

            await _assignmentTransferRequestRepository.AddAsync(assignmentTransferRequest);

            if (isOutsource && batch.UserId == null)
            {
                assignmentTransferRequest.MarkAsApproved();
                batch.ActiveNextAssignment(assignmentTransferRequest.Id, assignment.Id, quantityToSend, request.Note);
            }
            else
            {
                if (isOutsource)
                {
                    assignment.UpdateStatus("ReadyForTransfer");
                }
                assignmentTransferRequest.AddDomainEvent(new TransferRequestAddedEvent(request.UserId, assignment.Id));
            }
            

            return Result<Guid>.Success(assignmentTransferRequest.Id);
        }
    }
}
