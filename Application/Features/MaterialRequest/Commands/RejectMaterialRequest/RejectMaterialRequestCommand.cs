using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.MaterialRequest.Commands.RejectMaterialRequest
{
    public class RejectMaterialRequestCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string RejectedReason { get; set; } = string.Empty;
    }
}
