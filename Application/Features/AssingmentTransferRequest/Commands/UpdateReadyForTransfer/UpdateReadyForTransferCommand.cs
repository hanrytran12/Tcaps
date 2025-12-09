using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Assignments.Commands.UpdateReadyForTransfer
{
    public class UpdateReadyForTransferCommand : IRequest<Result<Guid>>
    {
        public Guid AssignmentId { get; set; }
        [JsonIgnore]
        public Guid QcId { get; set; }
    }
}