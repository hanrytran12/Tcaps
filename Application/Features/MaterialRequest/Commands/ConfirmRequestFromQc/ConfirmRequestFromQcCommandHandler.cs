using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc
{
    public class ConfirmRequestFromQcCommandHandler : IRequestHandler<ConfirmRequestFromQcCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public ConfirmRequestFromQcCommandHandler(IUnitOfWork unitOfWork, IMaterialRequestRepository materialRequestRepository)
        {
            _unitOfWork = unitOfWork;
            _materialRequestRepository = materialRequestRepository;
        }

        public async Task<Result> Handle(ConfirmRequestFromQcCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.Id);

            if (materialRequest is null)
            {
                return Result.Failure("Material request not found.");
            }

            if (materialRequest.QuantityRequest - request.ActualReceivedQuantity == 0)
            {
                materialRequest.MarkAsConfirmed(request.ActualReceivedQuantity, request.NoteFromQC);

            }
            else
            {
                materialRequest.MarkAsConfirmedWithDiscrepancy(request.ActualReceivedQuantity, request.NoteFromQC);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
