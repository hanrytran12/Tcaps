using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAssignmentByBatchId
{
    public class GetAssignmentByBatchIdQuery : IRequest<AssignForStaffDTO>
    {
        public Guid BatchId { get; set; }
        public Guid StaffId { get; set; }
    }
}
