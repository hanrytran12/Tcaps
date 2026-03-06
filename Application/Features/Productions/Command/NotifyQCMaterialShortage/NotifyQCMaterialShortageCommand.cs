using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Productions.Command.NotifyQCMaterialShortage
{
    public class NotifyQCMaterialShortageCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid StaffId { get; set; }

        public Guid AssignId { get; set; }
        public Guid MaterialId { get; set; }
        public decimal QuantityRemaining { get; set; }
    }
}
