using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ApproveRequestFromLead
{
    public class ApproveRequestFromLeadCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
