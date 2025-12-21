using System.Text.Json.Serialization;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest
{
    public class GetAllTransferRequestQuery : IRequest<List<AssignmentTransferRequestDTO>>
    {
        [JsonIgnore]
        public Guid LeadId { get; set; }

        public GetAllTransferRequestQuery(Guid leadId)
        {
            LeadId = leadId;
        }
    }
}
