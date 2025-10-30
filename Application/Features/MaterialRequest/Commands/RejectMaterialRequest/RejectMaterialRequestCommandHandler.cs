using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.RejectMaterialRequest
{
    public class RejectMaterialRequestCommandHandler : IRequestHandler<RejectMaterialRequestCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRequestRepository _repository;

        public RejectMaterialRequestCommandHandler(IUnitOfWork unitOfWork, IMaterialRequestRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Result> Handle(RejectMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _repository.GetByIdAsync(request.Id);

            if (materialRequest is null)
            {
                Result.Failure("MaterialRequest is not exist.");
            }

            materialRequest.MarkAdRejected(request.RejectedReason);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
