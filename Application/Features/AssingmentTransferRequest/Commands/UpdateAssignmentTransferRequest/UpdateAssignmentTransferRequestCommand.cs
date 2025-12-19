using System.Text.Json.Serialization;
using Application.Common;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Commands.UpdateAssignmentTransferRequest
{
    public class UpdateAssignmentTransferRequestCommand : IRequest<Result>
    {
        public Guid TransferRequestId { get; set; }
        [JsonIgnore]
        public Guid SupplierId { get; set; }
        public decimal CompleteQuantityReceive { get; set; }
        public string NoteLead { get; set; }

        public UpdateAssignmentTransferRequestCommand(Guid transferRequestId, Guid supplierId, decimal completeQuantityReceive, string noteLead)
        {
            TransferRequestId = transferRequestId;
            SupplierId = supplierId;
            CompleteQuantityReceive = completeQuantityReceive;
            NoteLead = noteLead;
        }
    }
}
