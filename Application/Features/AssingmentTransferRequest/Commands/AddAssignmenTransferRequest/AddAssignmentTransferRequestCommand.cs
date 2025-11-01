using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.AssingmentTransferRequest.Commands.AddAssignmenTransferRequest
{
    public class AddAssignmentTransferRequestCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid AssignmentId { get; set; }
        public string? Note { get; set; }
    }
}
