using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAssignmentsByStaffId
{
    public class GetAssignmentsByStaffIdQuery : IRequest<List<AssignForStaffDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
