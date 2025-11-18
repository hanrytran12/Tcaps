using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport
{
    public class GetAssignmentTransferForQcTransportQueryHandler : IRequestHandler<GetAssignmentTransferForQcTransportQuery, Result<AssignmentTransferRequest>>
    {
        private readonly IAssignmentTransferRequestRepository _repository;

        public GetAssignmentTransferForQcTransportQueryHandler(IAssignmentTransferRequestRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<AssignmentTransferRequest>> Handle(GetAssignmentTransferForQcTransportQuery request, CancellationToken cancellationToken)
        {
            var assignmentTransferRequest = await _repository.GetByIdAsync(request.AssignmentTransferRequestId);
            if (assignmentTransferRequest == null)
            {
                return Result<AssignmentTransferRequest>.Failure("Assignment Transfer Request not found.");
            }
            return Result<AssignmentTransferRequest>.Success(assignmentTransferRequest);
        }
    }
}
