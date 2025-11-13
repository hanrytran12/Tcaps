using System.Text.Json.Serialization;
using Application.Common;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest
{
    public class UpdateAssignmentTransferRequestCommand : IRequest<Result>
    {
        public Guid TransferRequestId { get; set; }
        [JsonIgnore]
        public Guid QcTransportId { get; set; }

        public UpdateAssignmentTransferRequestCommand(Guid transferRequestId, Guid qcTransportId)
        {
            TransferRequestId = transferRequestId;
            QcTransportId = qcTransportId;
        }
    }
}
