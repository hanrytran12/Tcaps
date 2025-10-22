using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.UpdateMaterialRequest
{
    public class UpdateMaterialRequestCommandHandler : IRequestHandler<UpdateMaterialRequestCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public UpdateMaterialRequestCommandHandler(IUnitOfWork unitOfWork, IMaterialRequestRepository materialRequestRepository)
        {
            _unitOfWork = unitOfWork;
            _materialRequestRepository = materialRequestRepository;
        }

        public async Task<Result> Handle(UpdateMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.Id);
            if (materialRequest is null)
            {
                return Result.Failure("Material request not found.");
            }

            try
            {
                materialRequest.MarkAsApproved();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
