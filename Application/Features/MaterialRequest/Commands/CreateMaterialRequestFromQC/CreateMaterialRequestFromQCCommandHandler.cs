using Application.Common;
using Application.Common.Exceptions;
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
                throw new NotFoundException("không tìm thấy phân công này.");

            //var today = DateOnly.FromDateTime(DateTime.Now);

            //if (today < assignment.StartDate || today > assignment.EndDate)
            //{
            //    throw new BadRequestException($"Ngày yêu cầu ({today:dd/MM/yyyy}) phải nằm trong khoảng từ {assignment.StartDate:dd/MM/yyyy} đến {assignment.EndDate:dd/MM/yyyy}.");
            //}

            //if (today > assignment.EndDate.AddDays(-2))
            //{
            //    throw new BadRequestException("Không thể tạo yêu cầu vật liệu vì đã quá sát ngày kết thúc (trước EndDate dưới 2 ngày).");
            //}

            foreach (var item in request.Items)
            {
                var materialRequest = Domain.Entities.MaterialRequest.Create(
                    item.MaterialId,
                    request.UserId,
                    assignment.BatchId,
                    request.AssignId,
                    item.Quantity,
                    request.Note,
                    "QcAddMaterial");

                if (materialRequest == null)
                    throw new BadRequestException("Failed to create material request");

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
