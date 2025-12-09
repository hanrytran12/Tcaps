using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQuery : IRequest<List<EvaluateDTO>>
    {
        [JsonIgnore]
        public Guid QC_Id { get; set; }
        public string? Status { get; set; }
    }
}
