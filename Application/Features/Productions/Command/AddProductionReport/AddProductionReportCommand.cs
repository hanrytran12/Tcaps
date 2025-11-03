using Application.Common;
using Application.DTOs.Request;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Productions.Command.AddProductionReport
{
    public class AddProductionReportCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid StaffId { get; set; }

        public Guid AssignId { get; set; }
        public int Quantity { get; set; }
        public List<MaterialUsageInputDTO> MaterialUsed { get; set; } = new();
    }
}
