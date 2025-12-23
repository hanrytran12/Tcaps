using Application.Common;
using Application.DTOs.Request;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest
{
    public class AddAssignmentTransferRequestCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        public Guid AssignmentId { get; set; }
        public Guid? ReworkRequestId { get; set; }
        public string? Note { get; set; }
        public decimal? QuantityCompletedSend { get; set; }
        public List<MaterialReconciliationDTO>? ReconciliationMaterials { get; set; } = new();
    }
}
