using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId
{
    public class GetDetailAssignmentByBatchIdQuery : IRequest<List<DashboardAssignmentDTO>>
    {
        public Guid BatchId { get; set; }
    }
}
