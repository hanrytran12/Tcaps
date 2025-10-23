using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialUse.Commands.AddMaterialUse
{
    public class AddMaterialUseCommandHandler : IRequestHandler<AddMaterialUseCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialRepository _materialRepository;

        public AddMaterialUseCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository, IMaterialRepository materialRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
            _materialRepository = materialRepository;
        }

        public async Task<Result<Guid>> Handle(AddMaterialUseCommand request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetByIdAsync(request.MaterialId);
            if (material is null)
            {
                return Result<Guid>.Failure("Material not found.");
            }

            var batch = await _batchRepository.GetByIdWithAssignmentsAsync(request.BatchId);
            if (batch is null)
            {
                return Result<Guid>.Failure("Batch not found.");
            }

            var assignments = batch.Assignments.FirstOrDefault(a => a.Id == request.AssignId);
            if (assignments is null)
            {
                return Result<Guid>.Failure("Assignment not found in the specified batch.");
            }

            var materialUse = Domain.Entities.MaterialUse.Create(request.MaterialId, request.BatchId, request.AssignId, request.QuantityDivide);
            batch.AddMaterialUse(materialUse);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(materialUse.Id);
        }
    }
}
