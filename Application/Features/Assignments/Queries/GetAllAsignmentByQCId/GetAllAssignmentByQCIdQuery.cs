using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAllAsignmentByQCId
{
    public class GetAllAssignmentByQCIdQuery : IRequest<List<AssignForStaffDTO>>
    {
        public Guid QcId { get; set; }
    }
}
