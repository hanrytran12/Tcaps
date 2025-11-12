using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.ReworkRequest.Commands.ApproveReworkRequest
{
    public class ApproveReworkRequestCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid RequestId { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly NextStepDeliveryDate { get; set; }
    }
}