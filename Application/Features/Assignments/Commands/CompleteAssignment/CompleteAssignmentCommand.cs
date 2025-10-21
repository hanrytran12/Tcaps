using Application.Common;
using MediatR;

namespace Application.Features.Assignments.Commands.CompleteAssignment
{
    public class CompleteAssignmentCommand : IRequest<Result>
    {
        public Guid AssignmentId { get; set; }
    }
}
