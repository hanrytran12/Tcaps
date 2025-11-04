using Application.Common;
using Application.DTOs.Request;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.MaterialRequest.Commands.DispatchRequest
{
    public class DispatchRequestCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public Guid AssignmentId { get; set; }

        public List<MaterialRequestItemDTO> Items { get; set; } = new();
    }
}
