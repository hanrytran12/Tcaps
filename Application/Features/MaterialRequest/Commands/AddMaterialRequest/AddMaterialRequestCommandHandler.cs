using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommandHandler : IRequestHandler<AddMaterialRequestCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public AddMaterialRequestCommandHandler(IMaterialRequestRepository materialRequestRepository, IBatchRepository batchRepository, IMaterialRepository materialRepository, IAssignmentRepository assignmentRepository)
        {
            _materialRequestRepository = materialRequestRepository;
            _batchRepository = batchRepository;
            _materialRepository = materialRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<Result<Guid>> Handle(AddMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var isAssignmentExist = await _assignmentRepository.ExistsAndBelongsToBatchAsync(request.AssignId, request.BatchId);
            if (!isAssignmentExist)
            {
                return Result<Guid>.Failure("Assignment is not exist on batch");
            }

            var material = await _materialRepository.GetByIdAsync(request.MaterialId);
            if (material is null)
            {
                return Result<Guid>.Failure("Material is not exist.");
            }

            var batch = await _batchRepository.GetByIdAsync(request.BatchId);
            if (batch is null)
            {
                return Result<Guid>.Failure("Batch is not exist.");
            }

            var materialRequest = Domain.Entities.MaterialRequest.Create(request.MaterialId, request.UserId, request.BatchId, request.AssignId, request.QuantityRequest, request.Note);
            await _materialRequestRepository.AddAsync(materialRequest);
            return Result<Guid>.Success(materialRequest.Id);
        }
    }
}
