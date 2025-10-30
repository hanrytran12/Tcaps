using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommandHandler : IRequestHandler<AddMaterialRequestCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        public AddMaterialRequestCommandHandler(IMaterialRequestRepository materialRequestRepository, IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _materialRequestRepository = materialRequestRepository;
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task<Result<Guid>> Handle(AddMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdWithAssignmentsAsync(request.BatchId);
            var assignment = batch.Assignments.FirstOrDefault(x => x.Id == request.AssignId);

            if (assignment is null)
            {
                return Result<Guid>.Failure("Assignment is not exist on batch");
            }

            var materialRequest = Domain.Entities.MaterialRequest.Create(request.MaterialId, request.UserId, request.BatchId, request.AssignId, request.QuantityRequest, request.Note);
            await _materialRequestRepository.AddAsync(materialRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(materialRequest.Id);
        }
    }
}
