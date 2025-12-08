using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.NewFolder
{
    public class GetAssignmentForHistoryByBatchIdQuery : IRequest<List<AssignmentHistoryDTO>>
    {
        public Guid UserId { get; set; }
        public Guid BatchId { get; set; }
    }
}
