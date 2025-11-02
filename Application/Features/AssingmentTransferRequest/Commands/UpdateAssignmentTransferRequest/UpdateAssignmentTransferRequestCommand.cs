using Application.Common;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest
{
    public class UpdateAssignmentTransferRequestCommand : IRequest<Result>
    {
        public Guid TransferRequestId { get; set; }

        public UpdateAssignmentTransferRequestCommand(Guid transferRequestId)
        {
            TransferRequestId = transferRequestId;
        }
    }
}
