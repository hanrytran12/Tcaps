using Application.Common;
using MediatR;

namespace Application.Features.ReworkRequest.Commands.ApproveReworkRequest
{
    public class ApproveReworkRequestCommand : IRequest<Result>
    {
        public Guid RequestId { get; set; }
        public DateOnly DeliveryMaterial { get; set; }

        public ApproveReworkRequestCommand(Guid requestId, DateOnly deliveryMaterial)
        {
            RequestId = requestId;
            DeliveryMaterial = deliveryMaterial;
        }
    }
}