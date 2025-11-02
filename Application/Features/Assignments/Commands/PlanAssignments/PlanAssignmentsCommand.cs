using Application.Common;
using Application.DTOs.Request;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Assignments.Commands.PlanAssignments
{
    public class PlanAssignmentsCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid BatchId { get; set; }

        public List<AssignmentPlanItemDTO> PlanItems { get; set; } = new();
    }
}
