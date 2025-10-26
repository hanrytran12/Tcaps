using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommand : IRequest<Result<Guid>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityRequest { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
