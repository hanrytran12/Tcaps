using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByAssignId
{
    public class GetReworkByAssignIdQueryHandler : IRequestHandler<GetReworkByAssignIdQuery, Result<Domain.Entities.ReworkRequest>>
    {
        private readonly IReworkRequestRepository _reworkRequestRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public GetReworkByAssignIdQueryHandler(IReworkRequestRepository reworkRequestRepository, IAssignmentRepository assignmentRepository)
        {
            _reworkRequestRepository = reworkRequestRepository;
            _assignmentRepository = assignmentRepository;
        }
        public async Task<Result<Domain.Entities.ReworkRequest>> Handle(GetReworkByAssignIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignId);
            if (assignment == null)
            {
                return Result<Domain.Entities.ReworkRequest>.Failure("Assignment not found.");
            }

            if (assignment.Status != "Reworking")
                return Result<Domain.Entities.ReworkRequest>.Failure("Assignment không có yêu cầu làm lại.");

            var reworkRequest = await _reworkRequestRepository.GetByAssignIdAsync(request.AssignId);
            if (reworkRequest == null)
                return Result<Domain.Entities.ReworkRequest>.Failure("Rework request not found.");

            return Result<Domain.Entities.ReworkRequest>.Success(reworkRequest);
        }
    }
}
