using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommandHandler : IRequestHandler<AddMaterialRequestCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddMaterialRequestCommandHandler(IMaterialRequestRepository materialRequestRepository, IUnitOfWork unitOfWork)
        {
            _materialRequestRepository = materialRequestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = Domain.Entities.MaterialRequest.Create(request.MaterialId, request.UserId, request.BatchId, request.QuantityRequest, request.Note);
            await _materialRequestRepository.AddAsync(materialRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(materialRequest.Id);
        }
    }
}
