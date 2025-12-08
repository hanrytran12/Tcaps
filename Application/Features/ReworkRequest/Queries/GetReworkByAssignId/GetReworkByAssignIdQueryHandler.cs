using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByAssignId
{
    public class GetReworkByAssignIdQueryHandler : IRequestHandler<GetReworkByAssignIdQuery, Domain.Entities.ReworkRequest>
    {
        private readonly IReworkRequestRepository _reworkRequestRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public GetReworkByAssignIdQueryHandler(IReworkRequestRepository reworkRequestRepository, IAssignmentRepository assignmentRepository)
        {
            _reworkRequestRepository = reworkRequestRepository;
            _assignmentRepository = assignmentRepository;
        }
        public async Task<Domain.Entities.ReworkRequest> Handle(GetReworkByAssignIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignId);
            if (assignment is null)
            {
                throw new NotFoundException("Assignment not found.");
            }

            if (assignment.Status != "Reworking")
            {
                throw new ConflictException("Assignment không có yêu cầu làm lại.");
            }

            var reworkRequest = await _reworkRequestRepository.GetByAssignIdAsync(request.AssignId);
            if (reworkRequest is null)
            {
                throw new NotFoundException("Rework request not found.");
            }

            return reworkRequest;
        }
    }
}
