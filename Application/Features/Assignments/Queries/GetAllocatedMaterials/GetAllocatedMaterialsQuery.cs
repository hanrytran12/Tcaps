using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAllocatedMaterials
{
    public class GetAllocatedMaterialsQuery : IRequest<List<AllocatedMaterialDto>>
    {
        public Guid AssignmentId { get; set; }

        public GetAllocatedMaterialsQuery(Guid assignmentId)
        {
            AssignmentId = assignmentId;
        }
    }
}
