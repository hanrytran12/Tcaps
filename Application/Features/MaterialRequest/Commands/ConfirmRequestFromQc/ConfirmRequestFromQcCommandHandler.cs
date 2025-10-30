using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc
{
    public class ConfirmRequestFromQcCommandHandler : IRequestHandler<ConfirmRequestFromQcCommand, Result>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public ConfirmRequestFromQcCommandHandler(IMaterialRequestRepository materialRequestRepository)
        {
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

            return Result.Success();
        }
    }
}
