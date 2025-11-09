using Application.Common;
using MediatR;

namespace Application.Features.ReworkRequest.Commands.RejectReworkRequest
{
    public class RejectRequestReworkCommand : IRequest<Result>
    {
        public Guid RequestId { get; set; }

        public RejectRequestReworkCommand(Guid requestId)
        {
            RequestId = requestId;
        }
    }
}
