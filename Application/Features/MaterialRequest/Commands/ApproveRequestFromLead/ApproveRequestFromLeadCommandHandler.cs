using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ApproveRequestFromLead
{
    public class ApproveRequestFromLeadCommandHandler : IRequestHandler<ApproveRequestFromLeadCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public ApproveRequestFromLeadCommandHandler(IUnitOfWork unitOfWork, IMaterialRequestRepository materialRequestRepository)
        {
            _unitOfWork = unitOfWork;
            _materialRequestRepository = materialRequestRepository;
        }

        public async Task<Result> Handle(ApproveRequestFromLeadCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.Id);
            if (materialRequest is null)
            {
                throw new NotFoundException("Material request not found.");
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
