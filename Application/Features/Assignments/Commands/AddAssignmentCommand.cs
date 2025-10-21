using Application.Common;
using MediatR;

namespace Application.Features.Assignments.Commands
{
    public class AddAssignmentCommand : IRequest<Result<Guid>>
    {
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public int Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
