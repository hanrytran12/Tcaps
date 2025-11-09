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

namespace Application.Features.MaterialRequest.Commands.CreateMaterialRequestFromQC
{
    public class CreateMaterialRequestFromQCCommandHandler : IRequestHandler<CreateMaterialRequestFromQCCommand, Result>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IAssignmentRepository _assignmentRepository;

        public CreateMaterialRequestFromQCCommandHandler(IMaterialRequestRepository materialRequestRepository, IUnitOfWork unitOfWork, IMediator mediator, 
            IAssignmentRepository assignmentRepository)
        {
            _materialRequestRepository = materialRequestRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _assignmentRepository = assignmentRepository;
        }
        public async Task<Result> Handle(CreateMaterialRequestFromQCCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignId);
            if (assignment == null)
                return Result.Failure("không tìm thấy phân công này.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (today < assignment.StartDate || today > assignment.EndDate)
            {
                return Result.Failure($"Ngày yêu cầu ({today}) phải nằm trong khoảng từ {assignment.StartDate} đến {assignment.EndDate}.");
            }

            if (today > assignment.EndDate.AddDays(-2))
            {
                return Result.Failure("Không thể tạo yêu cầu vật liệu vì đã quá sát ngày kết thúc (trước EndDate dưới 2 ngày).");
            }

            foreach (var item in request.Items)
            {
                var materialRequest = Domain.Entities.MaterialRequest.Create(
                    item.MaterialId,
                    request.UserId,
                    assignment.BatchId,
                    request.AssignId,
                    item.Quantity,
                    request.Note);

                if (materialRequest == null)
                    return Result.Failure("Failed to create material request");

                await _materialRequestRepository.AddAsync(materialRequest);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CreateMaterialRequestEvent(
                request.UserId,
                assignment.BatchId,
                request.AssignId));
            return Result.Success();
        }
    }
}
